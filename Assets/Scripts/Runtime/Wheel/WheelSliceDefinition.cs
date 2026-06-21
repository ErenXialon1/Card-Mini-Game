using System;
using CardMiniGame.Rewards;
using UnityEngine;

namespace CardMiniGame.Wheel
{
    [Serializable]
    public class WheelSliceDefinition
    {
        public RewardDefinition Reward;
        public int AmountMultiplier = 1;
        public bool IsBomb;
    }
}
