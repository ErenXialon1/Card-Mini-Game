using CardMiniGame.Rewards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMiniGame.UI
{
    public class RewardItemView : MonoBehaviour
    {
        [SerializeField] private TMP_Text amountText;
        [SerializeField] private Image iconImage;
        [SerializeField] private UIAppearAnimator appearAnimator;

        public void Refresh(CollectedReward reward)
        {
            if (amountText != null)
            {
                string displayName = reward.Reward == null ? "Reward" : reward.Reward.DisplayName;
                amountText.text = displayName + " x" + reward.Amount;
            }

            if (iconImage != null)
            {
                iconImage.sprite = reward.Reward == null ? null : reward.Reward.Icon;
                iconImage.enabled = iconImage.sprite != null;
            }

            if (appearAnimator != null)
            {
                appearAnimator.Play();
            }
        }

        private void Awake()
        {
            BindAnimator();
        }

        private void Reset()
        {
            BindAnimator();
        }

        private void OnValidate()
        {
            BindAnimator();
        }

        private void BindAnimator()
        {
            if (appearAnimator == null)
            {
                appearAnimator = GetComponent<UIAppearAnimator>();
            }
        }
    }
}
