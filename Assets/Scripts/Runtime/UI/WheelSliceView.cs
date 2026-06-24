using CardMiniGame.Wheel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMiniGame.UI
{
    public class WheelSliceView : MonoBehaviour
    // Renders the UI for a single wheel slice.
    {
        [SerializeField] private GameObject root;
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text amountText;

        public void SetVisible(bool isVisible)
        {
            GameObject target = root == null ? gameObject : root;
            target.SetActive(isVisible);
        }

        public void Refresh(WheelSliceDefinition slice)
        {
            Refresh(slice, 1, 1f);
        }

        public void Refresh(WheelSliceDefinition slice, int zone, float rewardScalingPerZone)
        {
            if (slice == null)
            {
                SetVisible(false);
                return;
            }

            SetVisible(true);

            if (iconImage != null)
            {
                iconImage.sprite = slice.Reward == null ? null : slice.Reward.Icon;
                iconImage.enabled = iconImage.sprite != null;
            }

            if (amountText != null)
            {
                amountText.text = slice.IsBomb ? "BOMB" : GetAmountText(slice, zone, rewardScalingPerZone);
            }
        }

        private static string GetAmountText(WheelSliceDefinition slice, int zone, float rewardScalingPerZone)
        {
            int amount = WheelSpinResolver.GetScaledAmount(slice, zone, rewardScalingPerZone);

            if (amount <= 0)
            {
                return "0";
            }

            return amount.ToString();
        }
    }
}
