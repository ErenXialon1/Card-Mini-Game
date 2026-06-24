using System;
using CardMiniGame.Rewards;
using UnityEngine;

namespace CardMiniGame.Wheel
{
    // Stores the reward and behavior data for a wheel slice.
    [Serializable]
    public class WheelSliceDefinition
    {
        public RewardDefinition Reward;
        public int AmountMultiplier = 1;
        public bool IsBomb;
    }
}
