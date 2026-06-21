using System;
using DG.Tweening;
using UnityEngine;

namespace CardMiniGame.UI
{
    public class PopupScaleAnimator : MonoBehaviour
    {
        [SerializeField] private Transform animatedRoot;
        [SerializeField] private float duration = 0.2f;
        [SerializeField] private Ease showEase = Ease.OutBack;
        [SerializeField] private Ease hideEase = Ease.InBack;

        public void Show()
        {
            if (animatedRoot == null)
            {
                return;
            }

            animatedRoot.DOKill();
            animatedRoot.localScale = Vector3.zero;
            animatedRoot.DOScale(Vector3.one, Mathf.Max(0.01f, duration)).SetEase(showEase);
        }

        public void Hide(Action onComplete)
        {
            if (animatedRoot == null)
            {
                onComplete?.Invoke();
                return;
            }

            animatedRoot.DOKill();
            animatedRoot
                .DOScale(Vector3.zero, Mathf.Max(0.01f, duration))
                .SetEase(hideEase)
                .OnComplete(() => onComplete?.Invoke());
        }
    }
}
