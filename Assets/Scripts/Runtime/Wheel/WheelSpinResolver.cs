using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardMiniGame.Wheel
{
    public class WheelSpinResolver
    {
        public SpinResult Resolve(WheelConfig config, int zone, float rewardScalingPerZone)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (config.Slices == null || config.Slices.Count == 0)
            {
                throw new InvalidOperationException("WheelConfig has no slices.");
            }

            int selectedSliceIndex = Random.Range(0, config.Slices.Count);
            WheelSliceDefinition slice = config.Slices[selectedSliceIndex];

            if (slice == null)
            {
                throw new InvalidOperationException("Selected wheel slice is null.");
            }

            bool isBomb = slice.IsBomb;
            int amount = isBomb || slice.Reward == null
                ? 0
                : GetScaledAmount(slice, zone, rewardScalingPerZone);

            return new SpinResult(selectedSliceIndex, slice.Reward, amount, isBomb);
        }

        private static int GetScaledAmount(WheelSliceDefinition slice, int zone, float rewardScalingPerZone)
        {
            int safeZone = Mathf.Max(1, zone);
            float safeScaling = rewardScalingPerZone <= 0f ? 1f : rewardScalingPerZone;
            float scale = Mathf.Pow(safeScaling, safeZone - 1);
            return Mathf.RoundToInt(slice.Reward.BaseAmount * slice.AmountMultiplier * scale);
        }
    }
}
