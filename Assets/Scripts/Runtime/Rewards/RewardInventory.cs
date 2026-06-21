using System.Collections.Generic;

namespace CardMiniGame.Rewards
{
    public class RewardInventory
    {
        private readonly List<CollectedReward> rewards = new List<CollectedReward>();

        public IReadOnlyList<CollectedReward> Rewards => rewards;

        public void AddReward(RewardDefinition reward, int amount)
        {
            if (reward == null || amount <= 0)
            {
                return;
            }

            rewards.Add(new CollectedReward(reward, amount));
        }

        public void Clear()
        {
            rewards.Clear();
        }

        public int GetTotalValue()
        {
            int total = 0;

            for (int i = 0; i < rewards.Count; i++)
            {
                total += rewards[i].Amount;
            }

            return total;
        }
    }
}
