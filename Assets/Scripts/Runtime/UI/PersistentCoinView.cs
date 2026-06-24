using CardMiniGame.Rewards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMiniGame.UI
{
    public class PersistentCoinView : MonoBehaviour
    // Displays the persistent coin amount on screen.
    {
        [SerializeField] private TMP_Text coinText;
        [SerializeField] private Image iconImage;
        [SerializeField] private RewardDefinition coinReward;

        private void OnEnable()
        {
            PersistentInventory.Instance.Changed += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            PersistentInventory.Instance.Changed -= Refresh;
        }

        private void OnValidate()
        {
            ApplyIcon();
            Refresh();
        }

        public void Refresh()
        {
            if (coinText != null)
            {
                coinText.text = PersistentInventory.Instance.Coin.ToString();
            }

            ApplyIcon();
        }

        private void ApplyIcon()
        {
            if (iconImage == null)
            {
                return;
            }

            iconImage.sprite = coinReward == null ? null : coinReward.Icon;
            iconImage.enabled = iconImage.sprite != null;
            iconImage.raycastTarget = false;
        }
    }
}
