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
        }
    }
}
