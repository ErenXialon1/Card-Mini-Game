using System;
using System.Collections.Generic;
using CardMiniGame.Wheel;
using UnityEngine;
using UnityEngine.UI;

namespace CardMiniGame.UI
{
    public class WheelView : MonoBehaviour
    {
        [SerializeField] private Image wheelBaseImage;
        [SerializeField] private WheelSpinAnimator spinAnimator;
        [SerializeField] private List<WheelSliceView> sliceViews = new List<WheelSliceView>();

        public void Build(WheelConfig config)
        {
            if (config == null)
            {
                return;
            }

            if (wheelBaseImage != null)
            {
                wheelBaseImage.sprite = config.WheelBaseSprite;
                wheelBaseImage.color = config.VisualProfile == null ? Color.white : config.VisualProfile.WheelTint;
            }

            int configuredSliceCount = config.Slices == null ? 0 : config.Slices.Count;

            for (int i = 0; i < sliceViews.Count; i++)
            {
                WheelSliceView sliceView = sliceViews[i];

                if (sliceView == null)
                {
                    continue;
                }

                bool hasSlice = i < configuredSliceCount && config.Slices[i] != null;

                if (hasSlice)
                {
                    sliceView.Refresh(config.Slices[i]);
                }
                else
                {
                    sliceView.SetVisible(false);
                }
            }
        }

        public void SpinToSlice(WheelConfig config, int selectedSliceIndex, Action onComplete)
        {
            if (spinAnimator == null)
            {
                onComplete?.Invoke();
                return;
            }

            int sliceCount = config != null && config.Slices != null && config.Slices.Count > 0
                ? config.Slices.Count
                : Mathf.Max(1, sliceViews.Count);

            spinAnimator.PlaySpin(config == null ? null : config.VisualProfile, selectedSliceIndex, sliceCount, onComplete);
        }

        private void OnValidate()
        {
            if (sliceViews == null)
            {
                sliceViews = new List<WheelSliceView>();
            }
        }
    }
}
