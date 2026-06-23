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
        [SerializeField] private PopupScaleAnimator popupAnimator;
        [SerializeField] private RewardCardView bombCardView;
        [SerializeField] private RewardDefinition bombReward;

        public Button RestartButton => restartButton == null ? null : restartButton.Button;

        public void Show(int lostAmount)
        {
            gameObject.SetActive(true);
            popupAnimator?.Show();

            if (titleText != null)
            {
                titleText.text = "OH NO, A BOMB EXPLODED RIGHT IN YOUR HANDS!";
            }

            if (descriptionText != null)
            {
                descriptionText.text = "You lost " + lostAmount + " rewards.";
            }

            if (bombCardView != null)
            {
                bombCardView.Refresh(bombReward, 0, true);
                bombCardView.SetVisible(true);
            }

            if (continueOptionalButton != null)
            {
                continueOptionalButton.gameObject.SetActive(false);
            }
        }

        public void Hide()
        {
            if (popupAnimator == null || !gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
                return;
            }

            popupAnimator.Hide(() => gameObject.SetActive(false));
        }

        private void Awake()
        {
            if (continueOptionalButton != null)
            {
                continueOptionalButton.gameObject.SetActive(false);
            }
        }

        private void OnValidate()
        {
            if (continueOptionalButton == null)
            {
                return;
            }

            continueOptionalButton.gameObject.SetActive(false);
        }
    }
}
