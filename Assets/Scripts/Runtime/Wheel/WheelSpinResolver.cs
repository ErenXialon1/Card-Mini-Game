using System;
using Random = UnityEngine.Random;

namespace CardMiniGame.Wheel
{
    public class WheelSpinResolver
    {
        public SpinResult Resolve(WheelConfig config)
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
            int amount = isBomb || slice.Reward == null ? 0 : slice.Reward.BaseAmount * slice.AmountMultiplier;

            return new SpinResult(selectedSliceIndex, slice.Reward, amount, isBomb);
        }
    }
}
