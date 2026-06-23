using System;
using CardMiniGame.Wheel;
using DG.Tweening;
using UnityEngine;

namespace CardMiniGame.UI
{
    public class WheelSpinAnimator : MonoBehaviour
    {
        [SerializeField] private RectTransform animatedRoot;

        public void PlaySpin(SpinVisualProfile profile, int selectedSliceIndex, int sliceCount, Action onComplete)
        {
            if (animatedRoot == null || sliceCount <= 0)
            {
                onComplete?.Invoke();
                return;
            }

            float duration = profile.SpinDuration;
            int rotations = profile.MinimumFullRotations;
            float sliceAngle = 360f / sliceCount;
            float targetAngle = (Mathf.Max(1, rotations) * 360f) - (Mathf.Clamp(selectedSliceIndex, 0, sliceCount - 1) * sliceAngle);

            animatedRoot.DOKill();
            animatedRoot.localRotation = Quaternion.identity;
            animatedRoot
                .DORotate(new Vector3(0f, 0f, -targetAngle), Mathf.Max(0.01f, duration), RotateMode.FastBeyond360)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => onComplete?.Invoke());
        }
    }
}
