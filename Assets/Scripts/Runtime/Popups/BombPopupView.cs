using CardMiniGame.UI;
using CardMiniGame.Rewards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMiniGame.Popups
{
    public class BombPopupView : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private AutoButtonBinder restartButton;
        [SerializeField] private AutoButtonBinder continueOptionalButton;
        [SerializeField] private GameObject continueCostRoot;
        [SerializeField] private Image continueCostIcon;
        [SerializeField] private TMP_Text continueCostText;
        [SerializeField] private UIAppearAnimator appearAnimator;
        [SerializeField] private RewardCardView bombCardView;
        [SerializeField] private RewardDefinition bombReward;
        [SerializeField] private RewardDefinition coinReward;

        public Button RestartButton => restartButton == null ? null : restartButton.Button;
        public Button ContinueButton => continueOptionalButton == null ? null : continueOptionalButton.Button;

        public void Show(int lostAmount)
        {
            Show(lostAmount, 0, false);
        }

        public void Show(int lostAmount, int continueCost, bool canContinue)
        {
            gameObject.SetActive(true);
            appearAnimator?.Show();

            if (titleText != null)
            {
                titleText.text = "OH NO, A BOMB EXPLODED RIGHT IN YOUR HANDS!";
            }

            if (descriptionText != null)
            {
                descriptionText.text = BuildDescription(lostAmount, continueCost, canContinue);
            }

            if (bombCardView != null)
            {
                bombCardView.Refresh(bombReward, 0, true);
                bombCardView.SetVisible(true);
            }

            if (continueOptionalButton != null)
            {
                continueOptionalButton.gameObject.SetActive(continueCost > 0);

                if (continueOptionalButton.Button != null)
                {
                    continueOptionalButton.Button.interactable = canContinue;
                }
            }

            RefreshContinueCost(continueCost);
        }

        public void Hide()
        {
            if (appearAnimator == null || !gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
                return;
            }

            appearAnimator.Hide(() => gameObject.SetActive(false));
        }

        private void Awake()
        {
            if (continueOptionalButton != null)
            {
                continueOptionalButton.gameObject.SetActive(false);
            }

            if (continueCostRoot != null)
            {
                continueCostRoot.SetActive(false);
            }
        }

        private void OnValidate()
        {
            if (continueCostRoot != null)
            {
                continueCostRoot.SetActive(false);
            }

            if (continueOptionalButton != null)
            {
                continueOptionalButton.gameObject.SetActive(false);
            }
        }

        private void RefreshContinueCost(int continueCost)
        {
            bool hasContinueCost = continueCost > 0;

            if (continueCostRoot != null)
            {
                continueCostRoot.SetActive(hasContinueCost);
            }

            if (continueCostText != null)
            {
                continueCostText.text = hasContinueCost ? continueCost.ToString() : string.Empty;
            }

            if (continueCostIcon != null)
            {
                continueCostIcon.sprite = coinReward == null ? null : coinReward.Icon;
                continueCostIcon.enabled = hasContinueCost && continueCostIcon.sprite != null;
            }
        }

        private static string BuildDescription(int lostAmount, int continueCost, bool canContinue)
        {
            string description = "All collected amounts have been lost";

            if (continueCost <= 0)
            {
                return description;
            }

            if (canContinue)
            {
                return description + "Spend " + continueCost + " coins to revive";
            }

            return description + "You need " + continueCost + " coins to revive";
        }
    }
}
