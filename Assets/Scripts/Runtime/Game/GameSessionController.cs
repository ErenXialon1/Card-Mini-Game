using System;
using CardMiniGame.Popups;
using CardMiniGame.Rewards;
using CardMiniGame.UI;
using CardMiniGame.Wheel;
using CardMiniGame.Zones;
using UnityEngine;

namespace CardMiniGame.Game
{
    public class GameSessionController : MonoBehaviour
    {
        [SerializeField] private GameHudView hudView;
        [SerializeField] private RewardListView rewardListView;
        [SerializeField] private WheelView wheelView;
        [SerializeField] private BombPopupView bombPopupView;
        [SerializeField] private CashoutPopupView cashoutPopupView;
        [SerializeField] private GameFeedbackAudio feedbackAudio;

        private ZoneConfig zoneConfig;
        private GameSession session;
        private ZoneService zoneService;
        private WheelSpinResolver spinResolver;

        public void Initialize(
            ZoneConfig config,
            GameSession gameSession,
            ZoneService zones,
            WheelSpinResolver resolver)
        {
            zoneConfig = config;
            session = gameSession;
            zoneService = zones;
            spinResolver = resolver;
            HidePopups();
            ClearResultCard();
            RefreshAllViews();
            RegisterButtonListeners();
        }

        private void OnEnable()
        {
            RegisterButtonListeners();
        }

        private void Start()
        {
            RefreshAllViews();
        }

        private void OnDisable()
        {
            UnregisterButtonListeners();
        }

        private void RegisterButtonListeners()
        {
            UnregisterButtonListeners();

            if (hudView != null && hudView.SpinButton != null)
            {
                hudView.SpinButton.onClick.AddListener(HandleSpinClicked);
            }

            if (hudView != null && hudView.LeaveButton != null)
            {
                hudView.LeaveButton.onClick.AddListener(HandleLeaveClicked);
            }

            if (bombPopupView != null && bombPopupView.RestartButton != null)
            {
                bombPopupView.RestartButton.onClick.AddListener(HandleRestartClicked);
            }

            if (cashoutPopupView != null && cashoutPopupView.RestartButton != null)
            {
                cashoutPopupView.RestartButton.onClick.AddListener(HandleCashoutConfirmedClicked);
            }
        }

        private void UnregisterButtonListeners()
        {
            if (hudView != null && hudView.SpinButton != null)
            {
                hudView.SpinButton.onClick.RemoveListener(HandleSpinClicked);
            }

            if (hudView != null && hudView.LeaveButton != null)
            {
                hudView.LeaveButton.onClick.RemoveListener(HandleLeaveClicked);
            }

            if (bombPopupView != null && bombPopupView.RestartButton != null)
            {
                bombPopupView.RestartButton.onClick.RemoveListener(HandleRestartClicked);
            }

            if (cashoutPopupView != null && cashoutPopupView.RestartButton != null)
            {
                cashoutPopupView.RestartButton.onClick.RemoveListener(HandleCashoutConfirmedClicked);
            }
        }

        private void HandleSpinClicked()
        {
            if (!IsInitialized || session.SessionState != SessionState.Ready)
            {
                return;
            }

            WheelConfig wheelConfig = GetCurrentWheelConfig();

            if (wheelConfig == null)
            {
                Debug.LogWarning("No wheel config is assigned for the current zone.", this);
                return;
            }

            SpinResult spinResult;

            try
            {
                float rewardScaling = zoneConfig == null ? 1f : zoneConfig.RewardScalingPerZone;
                spinResult = spinResolver.Resolve(wheelConfig, session.CurrentZone, rewardScaling);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message, this);
                return;
            }

            session.SessionState = SessionState.Spinning;
            RefreshAllViews();

            if (wheelView == null)
            {
                CompleteSpin(spinResult);
                return;
            }

            wheelView.SpinToResult(wheelConfig, spinResult, () => CompleteSpin(spinResult));
        }

        private void HandleLeaveClicked()
        {
            if (!IsInitialized || session.SessionState != SessionState.Ready)
            {
                return;
            }

            bool isSpinning = session.SessionState == SessionState.Spinning;

            if (zoneService == null || !zoneService.IsLeaveAllowed(session.CurrentZone, isSpinning))
            {
                return;
            }

            if (cashoutPopupView != null)
            {
                cashoutPopupView.Show(session.CollectedRewards);
                return;
            }

            Debug.LogWarning("CashoutPopupView reference is missing.", this);
        }

        private void HandleRestartClicked()
        {
            if (!IsInitialized)
            {
                return;
            }

            session.Restart();
            HidePopups();
            ClearResultCard();
            RefreshAllViews();
        }

        private void HandleCashoutConfirmedClicked()
        {
            if (!IsInitialized)
            {
                return;
            }

            PersistentInventory.Instance.AddRewards(session.CollectedRewards);
            HandleRestartClicked();
        }

        private void CompleteSpin(SpinResult spinResult)
        {
            if (!IsInitialized)
            {
                return;
            }

            if (spinResult.IsBomb)
            {
                int lostAmount = session.CollectedRewards.GetTotalValue();
                session.LoseAllRewards();
                feedbackAudio?.PlayBomb();
                RefreshAllViews();

                if (bombPopupView != null)
                {
                    bombPopupView.Show(lostAmount);
                }

                return;
            }

            session.AddReward(spinResult.Reward, spinResult.Amount);
            feedbackAudio?.PlayReward(spinResult.Reward);
            session.AdvanceZone();
            feedbackAudio?.PlayZoneChanged(zoneService == null ? WheelType.Normal : zoneService.GetZoneType(session.CurrentZone));
            session.SessionState = SessionState.Ready;
            RefreshAllViews();
        }

        private void RefreshAllViews()
        {
            if (!IsInitialized)
            {
                return;
            }

            WheelType zoneType = zoneService == null ? WheelType.Normal : zoneService.GetZoneType(session.CurrentZone);
            bool isReady = session.SessionState == SessionState.Ready;
            bool canLeave = isReady && zoneService != null && zoneService.IsLeaveAllowed(session.CurrentZone, false);

            if (hudView != null)
            {
                hudView.Refresh(
                    session.CurrentZone,
                    zoneType,
                    session.CollectedRewards.GetTotalValue(),
                    isReady,
                    canLeave,
                    zoneConfig == null ? 5 : zoneConfig.SafeZoneInterval,
                    zoneConfig == null ? 30 : zoneConfig.SuperZoneInterval);
            }

            if (rewardListView != null)
            {
                rewardListView.UpdateList(session.CollectedRewards);
            }

            if (wheelView != null)
            {
                float rewardScaling = zoneConfig == null ? 1f : zoneConfig.RewardScalingPerZone;
                wheelView.Build(GetCurrentWheelConfig(), session.CurrentZone, rewardScaling);
            }
        }

        private WheelConfig GetCurrentWheelConfig()
        {
            if (zoneConfig == null || session == null)
            {
                return null;
            }

            WheelType zoneType = zoneService == null ? WheelType.Normal : zoneService.GetZoneType(session.CurrentZone);

            switch (zoneType)
            {
                case WheelType.Safe:
                    return zoneConfig.SafeWheel;
                case WheelType.Super:
                    return zoneConfig.SuperWheel;
                default:
                    return zoneConfig.NormalWheel;
            }
        }

        private void HidePopups()
        {
            if (bombPopupView != null)
            {
                bombPopupView.Hide();
            }

            if (cashoutPopupView != null)
            {
                cashoutPopupView.Hide();
            }
        }

        private void ClearResultCard()
        {
            if (wheelView != null)
            {
                wheelView.ClearResult();
            }
        }

        private bool IsInitialized => session != null && zoneService != null && spinResolver != null;
    }
}
