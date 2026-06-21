using CardMiniGame.Rewards;

namespace CardMiniGame.Wheel
{
    public readonly struct SpinResult
    {
        public readonly int SelectedSliceIndex;
        public readonly RewardDefinition Reward;
        public readonly int Amount;
        public readonly bool IsBomb;

        public SpinResult(int selectedSliceIndex, RewardDefinition reward, int amount, bool isBomb)
        {
            SelectedSliceIndex = selectedSliceIndex;
            Reward = reward;
            Amount = amount;
            IsBomb = isBomb;
        }
    }
}
