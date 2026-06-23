using System;
using System.Collections.Generic;
using CardMiniGame.Wheel;
using UnityEngine;
using UnityEngine.UI;

namespace CardMiniGame.UI
{
    public class WheelView : MonoBehaviour
    {
        [SerializeField] private WheelConfig previewConfig;
        [SerializeField] private Image wheelBaseImage;
        [SerializeField] private Image pointerImage;
        [SerializeField] private WheelSpinAnimator spinAnimator;
        [SerializeField] private RewardCardView resultCardView;
        [SerializeField] private List<WheelSliceView> sliceViews = new List<WheelSliceView>();

        public void Build(WheelConfig config)
        {
            if (config == null)
            {
                return;
            }

            if (wheelBaseImage != null)
            {
                if (config.WheelBaseSprite != null)
                {
                    wheelBaseImage.sprite = config.WheelBaseSprite;
                }

                wheelBaseImage.color = config.VisualProfile == null ? Color.white : config.VisualProfile.WheelTint;
            }

            if (pointerImage != null)
            {
                if (config.PointerSprite != null)
                {
                    pointerImage.sprite = config.PointerSprite;
                }

                pointerImage.color = config.VisualProfile == null ? Color.white : config.VisualProfile.PointerTint;
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

        public void SpinToResult(WheelConfig config, SpinResult result, Action onComplete)
        {
            if (resultCardView != null)
            {
                resultCardView.SetVisible(false);
            }

            if (spinAnimator == null)
            {
                ShowResult(result);
                onComplete?.Invoke();
                return;
            }

            int sliceCount = config != null && config.Slices != null && config.Slices.Count > 0
                ? config.Slices.Count
                : Mathf.Max(1, sliceViews.Count);

            spinAnimator.PlaySpin(
                config == null ? null : config.VisualProfile,
                result.SelectedSliceIndex,
                sliceCount,
                () =>
                {
                    ShowResult(result);
                    onComplete?.Invoke();
                });
        }

        public void ClearResult()
        {
            if (resultCardView != null)
            {
                resultCardView.SetVisible(false);
            }
        }

        private void ShowResult(SpinResult result)
        {
            if (resultCardView == null)
            {
                return;
            }

            resultCardView.Refresh(result);
            resultCardView.SetVisible(true);
        }

        private void OnValidate()
        {
            if (sliceViews == null)
            {
                sliceViews = new List<WheelSliceView>();
            }

            if (previewConfig != null)
            {
                Build(previewConfig);
            }
        }
    }
}
