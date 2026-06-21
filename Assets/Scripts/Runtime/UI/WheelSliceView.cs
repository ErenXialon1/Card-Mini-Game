using CardMiniGame.Wheel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMiniGame.UI
{
    public class WheelSliceView : MonoBehaviour
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
                amountText.text = slice.IsBomb ? "BOMB" : GetAmountText(slice);
            }
        }

        private static string GetAmountText(WheelSliceDefinition slice)
        {
            if (slice.Reward == null)
            {
                return "0";
            }

            return (slice.Reward.BaseAmount * slice.AmountMultiplier).ToString();
        }
    }
}
