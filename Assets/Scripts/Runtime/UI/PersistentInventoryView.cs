using System.Collections.Generic;
using CardMiniGame.Rewards;
using UnityEngine;
using UnityEngine.UI;

namespace CardMiniGame.UI
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class PersistentInventoryView : MonoBehaviour
    // Shows the persistent inventory in a grid layout.
    {
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField] private Transform itemContainer;
        [SerializeField] private RewardItemView itemTemplate;
        [SerializeField] private List<RewardDefinition> rewardDefinitions = new List<RewardDefinition>();

        private readonly List<RewardItemView> spawnedItems = new List<RewardItemView>();

        private void Awake()
        {
            Bind();

            if (itemTemplate != null)
            {
                itemTemplate.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            PersistentInventory.Instance.Changed += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            PersistentInventory.Instance.Changed -= Refresh;
            ClearSpawnedItems();
        }

        private void Reset()
        {
            Bind();
        }

        private void OnValidate()
        {
            Bind();
        }

        public void Refresh()
        {
            ClearSpawnedItems();

            if (itemContainer == null || itemTemplate == null)
            {
                return;
            }

            IReadOnlyList<PersistentInventoryItem> items = PersistentInventory.Instance.Items;

            for (int i = 0; i < items.Count; i++)
            {
                PersistentInventoryItem item = items[i];

                if (item == null || item.Amount <= 0)
                {
                    continue;
                }

                RewardDefinition reward = GetRewardDefinition(item.RewardId);
                RewardItemView itemView = Instantiate(itemTemplate, itemContainer);
                itemView.gameObject.SetActive(true);
                itemView.Refresh(GetDisplayName(item, reward), reward == null ? null : reward.Icon, item.Amount);
                spawnedItems.Add(itemView);
            }
        }

        private RewardDefinition GetRewardDefinition(string rewardId)
        {
            if (string.IsNullOrEmpty(rewardId) || rewardDefinitions == null)
            {
                return null;
            }

            for (int i = 0; i < rewardDefinitions.Count; i++)
            {
                RewardDefinition reward = rewardDefinitions[i];

                if (reward != null && reward.RewardId == rewardId)
                {
                    return reward;
                }
            }

            return null;
        }

        private static string GetDisplayName(PersistentInventoryItem item, RewardDefinition reward)
        {
            if (reward != null && !string.IsNullOrEmpty(reward.DisplayName))
            {
                return reward.DisplayName;
            }

            return string.IsNullOrEmpty(item.RewardId) ? "Reward" : item.RewardId;
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

        private void Bind()
        {
            if (gridLayoutGroup == null)
            {
                gridLayoutGroup = GetComponent<GridLayoutGroup>();
            }

            if (itemContainer == null && gridLayoutGroup != null)
            {
                itemContainer = gridLayoutGroup.transform;
            }
        }
    }
}
