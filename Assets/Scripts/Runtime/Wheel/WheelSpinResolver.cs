using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardMiniGame.Wheel
{
    public class WheelSpinResolver
    {
        public SpinResult Resolve(WheelConfig config, int zone, float rewardScalingPerZone, float bombChance)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (config.Slices == null || config.Slices.Count == 0)
            {
                throw new InvalidOperationException("WheelConfig has no slices.");
            }

            int selectedSliceIndex = GetSelectedSliceIndex(config, bombChance);
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

        private static int GetSelectedSliceIndex(WheelConfig config, float bombChance)
        {
            List<int> bombSliceIndexes = new List<int>();
            List<int> rewardSliceIndexes = new List<int>();

            for (int i = 0; i < config.Slices.Count; i++)
            {
                WheelSliceDefinition slice = config.Slices[i];

                if (slice == null)
                {
                    continue;
                }

                if (slice.IsBomb)
                {
                    bombSliceIndexes.Add(i);
                }
                else
                {
                    rewardSliceIndexes.Add(i);
                }
            }

            if (bombSliceIndexes.Count == 0 && rewardSliceIndexes.Count == 0)
            {
                throw new InvalidOperationException("WheelConfig has no valid slices.");
            }

            float safeBombChance = Mathf.Clamp01(bombChance);

            if (bombSliceIndexes.Count > 0 && Random.value < safeBombChance)
            {
                return bombSliceIndexes[Random.Range(0, bombSliceIndexes.Count)];
            }

            if (rewardSliceIndexes.Count > 0)
            {
                return rewardSliceIndexes[Random.Range(0, rewardSliceIndexes.Count)];
            }

            return bombSliceIndexes[Random.Range(0, bombSliceIndexes.Count)];
        }

        public static int GetScaledAmount(WheelSliceDefinition slice, int zone, float rewardScalingPerZone)
        {
            if (slice == null || slice.IsBomb || slice.Reward == null)
            {
                return 0;
            }

            int safeZone = Mathf.Max(1, zone);
            float safeScaling = rewardScalingPerZone <= 0f ? 1f : rewardScalingPerZone;
            float scale = Mathf.Pow(safeScaling, safeZone - 1);
            return Mathf.RoundToInt(slice.Reward.BaseAmount * slice.AmountMultiplier * scale);
        }
    }
}
