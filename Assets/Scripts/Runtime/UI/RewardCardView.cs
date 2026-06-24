using CardMiniGame.Rewards;
using CardMiniGame.Wheel;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CardMiniGame.UI
{
    public class RewardCardView : MonoBehaviour, IPointerClickHandler
    // Displays the visual card for a spin result.
    {
        [SerializeField] private GameObject root;
        [SerializeField] private Image frameImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text amountText;
        [SerializeField] private UIAppearAnimator appearAnimator;
        [SerializeField] private bool hideOnPointerClick;
        [SerializeField] private Color rewardFrameColor = Color.white;
        [SerializeField] private Color bombFrameColor = new Color(1f, 0.15f, 0.1f, 1f);

        public void SetVisible(bool isVisible)
        {
            ApplyClickRaycastState();
            GameObject target = root == null ? gameObject : root;
            target.SetActive(isVisible);

            if (isVisible && appearAnimator != null)
            {
                appearAnimator.Play();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!hideOnPointerClick)
            {
                return;
            }

            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            SetVisible(false);
        }

        public void Refresh(SpinResult result)
        {
            Refresh(result.Reward, result.Amount, result.IsBomb);
        }

        public void Refresh(RewardDefinition reward, int amount, bool isBomb)
        {
            if (frameImage != null)
            {
                frameImage.color = isBomb ? bombFrameColor : rewardFrameColor;
            }

            if (iconImage != null)
            {
                iconImage.sprite = reward == null ? null : reward.Icon;
                iconImage.enabled = iconImage.sprite != null;
            }

            if (titleText != null)
            {
                titleText.text = isBomb ? "BOMB" : GetRewardTitle(reward);
            }

            if (amountText != null)
            {
                amountText.text = isBomb || amount <= 0 ? string.Empty : "x" + amount;
            }
        }

        private static string GetRewardTitle(RewardDefinition reward)
        {
            if (reward == null)
            {
                return "REWARD";
            }

            if (!string.IsNullOrEmpty(reward.DisplayName))
            {
                return reward.DisplayName.ToUpperInvariant();
            }

            return string.IsNullOrEmpty(reward.RewardId) ? "REWARD" : reward.RewardId.ToUpperInvariant();
        }

        private void Awake()
        {
            BindAnimator();
            ApplyClickRaycastState();
        }

        private void Reset()
        {
            BindAnimator();
            ApplyClickRaycastState();
        }

        private void OnValidate()
        {
            BindAnimator();
            ApplyClickRaycastState();
        }

        private void BindAnimator()
        {
            if (appearAnimator == null)
            {
                appearAnimator = GetComponent<UIAppearAnimator>();
            }
        }

        private void ApplyClickRaycastState()
        {
            if (frameImage != null)
            {
                frameImage.raycastTarget = hideOnPointerClick;
            }
        }
    }
}
