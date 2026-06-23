using System.Collections.Generic;
using CardMiniGame.Rewards;
using UnityEngine;

namespace CardMiniGame.UI
{
    public class RewardListView : MonoBehaviour
    {
        [SerializeField] private Transform rewardListContainer;
        [SerializeField] private RewardItemView rewardItemTemplate;

        private readonly List<RewardItemView> spawnedItems = new List<RewardItemView>();

        public void UpdateList(RewardInventory inventory)
        {
            ClearSpawnedItems();

            if (inventory == null || rewardListContainer == null || rewardItemTemplate == null)
            {
                return;
            }

            IReadOnlyList<CollectedReward> rewards = inventory.Rewards;
            List<CollectedReward> aggregatedRewards = new List<CollectedReward>();

            for (int i = 0; i < rewards.Count; i++)
            {
                AddAggregated(aggregatedRewards, rewards[i]);
            }

            for (int i = 0; i < aggregatedRewards.Count; i++)
            {
                RewardItemView item = Instantiate(rewardItemTemplate, rewardListContainer);
                item.gameObject.SetActive(true);
                spawnedItems.Add(item);
                item.Refresh(aggregatedRewards[i]);
            }
        }

        private void Awake()
        {
            if (rewardItemTemplate != null)
            {
                rewardItemTemplate.gameObject.SetActive(false);
            }
        }

        private void ClearSpawnedItems()
        {
            for (int i = 0; i < spawnedItems.Count; i++)
            {
                RewardItemView item = spawnedItems[i];

                if (item == null)
                {
                    continue;
                }

                if (Application.isPlaying)
                {
                    Destroy(item.gameObject);
                }
                else
                {
                    DestroyImmediate(item.gameObject);
                }
            }

            spawnedItems.Clear();
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
    }
}
