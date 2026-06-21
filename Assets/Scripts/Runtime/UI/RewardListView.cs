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

            for (int i = 0; i < rewards.Count; i++)
            {
                RewardItemView item = Instantiate(rewardItemTemplate, rewardListContainer);
                item.gameObject.SetActive(true);
                spawnedItems.Add(item);
                item.Refresh(rewards[i]);
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
    }
}
