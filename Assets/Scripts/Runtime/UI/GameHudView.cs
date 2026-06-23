using CardMiniGame.Wheel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMiniGame.UI
{
    public class GameHudView : MonoBehaviour
    {
        [SerializeField] private TMP_Text zoneTitleText;
        [SerializeField] private TMP_Text zoneTypeText;
        [SerializeField] private TMP_Text totalRewardText;
        [SerializeField] private ZoneTrackView zoneTrackView;
        [SerializeField] private ZoneBadgeView zoneBadgeView;
        [SerializeField] private AutoButtonBinder spinButton;
        [SerializeField] private AutoButtonBinder leaveButton;

        public Button SpinButton => spinButton == null ? null : spinButton.Button;
        public Button LeaveButton => leaveButton == null ? null : leaveButton.Button;

        public void Refresh(
            int currentZone,
            WheelType zoneType,
            int totalReward,
            bool canSpin,
            bool canLeave,
            int safeZoneInterval,
            int superZoneInterval)
        {
            if (zoneTitleText != null)
            {
                zoneTitleText.text = "ZONE " + currentZone.ToString("00");
            }

            if (zoneTypeText != null)
            {
                zoneTypeText.text = zoneType.ToString().ToUpperInvariant();
            }

            if (totalRewardText != null)
            {
                totalRewardText.text = totalReward.ToString();
            }

            if (zoneTrackView != null)
            {
                zoneTrackView.Refresh(currentZone, safeZoneInterval, superZoneInterval);
            }

            if (zoneBadgeView != null)
            {
                zoneBadgeView.Refresh(currentZone, safeZoneInterval, superZoneInterval);
            }

            Button spin = SpinButton;

            if (spin != null)
            {
                spin.interactable = canSpin;
            }

            Button leave = LeaveButton;

            if (leave != null)
            {
                leave.interactable = canLeave;
            }
        }
    }
}
