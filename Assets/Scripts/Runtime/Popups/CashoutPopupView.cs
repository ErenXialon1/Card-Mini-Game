using System.Collections.Generic;
using CardMiniGame.UI;
using CardMiniGame.Rewards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMiniGame.Popups
{
    public class CashoutPopupView : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text amountText;
        [SerializeField] private RewardListView rewardListView;
        [SerializeField] private AutoButtonBinder restartButton;
        [SerializeField] private UIAppearAnimator appearAnimator;

        public Button RestartButton => restartButton == null ? null : restartButton.Button;

        public void Show(RewardInventory inventory)
        {
            gameObject.SetActive(true);
            appearAnimator?.Show();

            if (titleText != null)
            {
                titleText.text = "CASH OUT";
            }

            bool canUseRewardList = CanUseRewardListView();
            int renderedItemCount = 0;

            if (canUseRewardList)
            {
                renderedItemCount = rewardListView.UpdateList(inventory);
            }

            if (amountText != null)
            {
                amountText.text = canUseRewardList && renderedItemCount > 0
                    ? string.Empty
                    : GetFallbackRewardText(inventory);
            }
        }

        public void Hide()
        {
            if (CanUseRewardListView())
            {
                rewardListView.UpdateList(null);
            }

            if (appearAnimator == null || !gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
                return;
            }

            appearAnimator.Hide(() => gameObject.SetActive(false));
        }

        private static string GetFallbackRewardText(RewardInventory inventory)
        {
            if (inventory == null || inventory.Rewards == null || inventory.Rewards.Count == 0)
            {
                return "No rewards";
            }

            return BuildRewardLines(inventory);
        }

        private bool CanUseRewardListView()
        {
            return rewardListView != null
                && rewardListView.CanRender
                && rewardListView.transform.IsChildOf(transform);
        }

        private static string BuildRewardLines(RewardInventory inventory)
        {
            string text = string.Empty;
            List<CollectedReward> aggregatedRewards = new List<CollectedReward>();

            for (int i = 0; i < inventory.Rewards.Count; i++)
            {
                AddAggregated(aggregatedRewards, inventory.Rewards[i]);
            }

            for (int i = 0; i < aggregatedRewards.Count; i++)
            {
                CollectedReward reward = aggregatedRewards[i];

                if (reward.Amount <= 0)
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(text))
                {
                    text += "\n";
                }

                text += GetRewardName(reward) + " x" + reward.Amount;
            }

            return text;
        }

        private static void AddAggregated(List<CollectedReward> target, CollectedReward reward)
        {
            if (reward.Amount <= 0)
            {
                return;
            }

            for (int i = 0; i < target.Count; i++)
            {
                CollectedReward current = target[i];

                if (current.Reward == reward.Reward)
                {
                    target[i] = new CollectedReward(current.Reward, current.Amount + reward.Amount);
                    return;
                }
            }

            target.Add(reward);
        }

        private static string GetRewardName(CollectedReward reward)
        {
            if (reward.Reward == null)
            {
                return "Reward";
            }

            if (!string.IsNullOrEmpty(reward.Reward.DisplayName))
            {
                return reward.Reward.DisplayName;
            }

            return string.IsNullOrEmpty(reward.Reward.RewardId) ? "Reward" : reward.Reward.RewardId;
        }
    }
}
