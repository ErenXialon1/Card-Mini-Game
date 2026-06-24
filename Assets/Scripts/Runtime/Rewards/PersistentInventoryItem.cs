using System;

namespace CardMiniGame.Rewards
{
    // Represents a single entry in the persistent inventory.
    [Serializable]
    public class PersistentInventoryItem
    {
        public string RewardId;
        public int Amount;

        public PersistentInventoryItem()
        {
        }

        public PersistentInventoryItem(string rewardId, int amount)
        {
            RewardId = rewardId;
            Amount = amount;
        }
    }
}
