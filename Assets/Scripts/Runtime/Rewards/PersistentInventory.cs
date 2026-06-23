using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardMiniGame.Rewards
{
    public class PersistentInventory
    {
        public const string CoinRewardId = "Coin";

        private const string SaveKey = "CardMiniGame.PersistentInventory.v1";

        private static PersistentInventory instance;

        private readonly List<PersistentInventoryItem> items = new List<PersistentInventoryItem>();

        public static PersistentInventory Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }

                instance = new PersistentInventory();
                instance.Load();
                return instance;
            }
        }

        public IReadOnlyList<PersistentInventoryItem> Items => items;
        public int Coin => GetAmount(CoinRewardId);

        public event Action Changed;

        public void AddRewards(RewardInventory inventory)
        {
            if (inventory == null)
            {
                return;
            }

            bool changed = false;
            IReadOnlyList<CollectedReward> rewards = inventory.Rewards;

            for (int i = 0; i < rewards.Count; i++)
            {
                changed |= AddRewardInternal(rewards[i].Reward, rewards[i].Amount);
            }

            if (changed)
            {
                SaveAndNotify();
            }
        }

        public void AddReward(RewardDefinition reward, int amount)
        {
            if (AddRewardInternal(reward, amount))
            {
                SaveAndNotify();
            }
        }

        public bool TrySpend(string rewardId, int amount)
        {
            if (string.IsNullOrEmpty(rewardId) || amount <= 0)
            {
                return false;
            }

            PersistentInventoryItem item = FindItem(rewardId);

            if (item == null || item.Amount < amount)
            {
                return false;
            }

            item.Amount -= amount;

            if (item.Amount <= 0)
            {
                items.Remove(item);
            }

            SaveAndNotify();
            return true;
        }

        public bool TrySpendCoin(int amount)
        {
            return TrySpend(CoinRewardId, amount);
        }

        public int GetAmount(string rewardId)
        {
            PersistentInventoryItem item = FindItem(rewardId);
            return item == null ? 0 : item.Amount;
        }

        public void Clear()
        {
            items.Clear();
            PlayerPrefs.DeleteKey(SaveKey);
            PlayerPrefs.Save();
            Changed?.Invoke();
        }

        public void Load()
        {
            items.Clear();

            string json = PlayerPrefs.GetString(SaveKey, string.Empty);

            if (string.IsNullOrEmpty(json))
            {
                return;
            }

            try
            {
                InventorySaveData saveData = JsonUtility.FromJson<InventorySaveData>(json);

                if (saveData == null || saveData.Items == null)
                {
                    return;
                }

                for (int i = 0; i < saveData.Items.Count; i++)
                {
                    AddAmount(saveData.Items[i].RewardId, saveData.Items[i].Amount);
                }
            }
            catch (Exception exception)
            {
                Debug.LogWarning("Persistent inventory load failed: " + exception.Message);
            }
        }

        private bool AddRewardInternal(RewardDefinition reward, int amount)
        {
            if (reward == null || reward.RewardType == RewardType.Bomb || amount <= 0)
            {
                return false;
            }

            string rewardId = GetPersistentRewardId(reward);

            if (string.IsNullOrEmpty(rewardId))
            {
                return false;
            }

            AddAmount(rewardId, amount);
            return true;
        }

        private static string GetPersistentRewardId(RewardDefinition reward)
        {
            if (reward.RewardType == RewardType.Currency)
            {
                return CoinRewardId;
            }

            return reward.RewardId;
        }

        private void AddAmount(string rewardId, int amount)
        {
            if (string.IsNullOrEmpty(rewardId) || amount <= 0)
            {
                return;
            }

            PersistentInventoryItem item = FindItem(rewardId);

            if (item == null)
            {
                items.Add(new PersistentInventoryItem(rewardId, amount));
                return;
            }

            item.Amount += amount;
        }

        private PersistentInventoryItem FindItem(string rewardId)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].RewardId == rewardId)
                {
                    return items[i];
                }
            }

            return null;
        }

        private void SaveAndNotify()
        {
            Save();
            Changed?.Invoke();
        }

        private void Save()
        {
            InventorySaveData saveData = new InventorySaveData
            {
                Items = new List<PersistentInventoryItem>(items)
            };

            PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(saveData));
            PlayerPrefs.Save();
        }

        [Serializable]
        private class InventorySaveData
        {
            public List<PersistentInventoryItem> Items = new List<PersistentInventoryItem>();
        }
    }
}
