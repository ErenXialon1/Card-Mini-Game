using CardMiniGame.Rewards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMiniGame.UI
{
    public class RewardItemView : MonoBehaviour
    // Shows a single reward entry inside the list.
    {
        [SerializeField] private TMP_Text amountText;
        [SerializeField] private Image iconImage;
        [SerializeField] private UIAppearAnimator appearAnimator;

        public void Refresh(CollectedReward reward)
        {
            string displayName = GetDisplayName(reward.Reward);
            Sprite icon = reward.Reward == null ? null : reward.Reward.Icon;
            Refresh(displayName, icon, reward.Amount);
        }

        public void Refresh(string displayName, Sprite icon, int amount)
        {
            if (amountText != null)
            {
                amountText.text = displayName + " x" + amount;
            }

            if (iconImage != null)
            {
                iconImage.sprite = icon;
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

        private static string GetDisplayName(RewardDefinition reward)
        {
            if (reward == null)
            {
                return "Reward";
            }

            if (!string.IsNullOrEmpty(reward.DisplayName))
            {
                return reward.DisplayName;
            }

            return string.IsNullOrEmpty(reward.RewardId) ? "Reward" : reward.RewardId;
        }
    }
}
