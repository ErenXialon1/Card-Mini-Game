namespace CardMiniGame.Rewards
{
    public readonly struct CollectedReward
    {
        public readonly RewardDefinition Reward;
        public readonly int Amount;

        public CollectedReward(RewardDefinition reward, int amount)
        {
            Reward = reward;
            Amount = amount;
        }
    }
}
