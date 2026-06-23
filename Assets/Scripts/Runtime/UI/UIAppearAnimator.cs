using System;
using DG.Tweening;
using UnityEngine;

namespace CardMiniGame.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasGroup))]
    public class UIAppearAnimator : MonoBehaviour
    {
        [SerializeField] private Transform animatedRoot;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private bool playOnEnable;
        [SerializeField] private bool useFade = true;
        [SerializeField] private bool useScale = true;
        [SerializeField] private Vector3 hiddenScale = new Vector3(0.92f, 0.92f, 1f);
        [SerializeField] private Vector3 visibleScale = Vector3.one;
        [SerializeField] private float showDuration = 0.2f;
        [SerializeField] private float hideDuration = 0.14f;
        [SerializeField] private Ease showEase = Ease.OutBack;
        [SerializeField] private Ease hideEase = Ease.InQuad;
        [SerializeField] private bool useUnscaledTime = true;

        private Sequence sequence;

        public void Show()
        {
            gameObject.SetActive(true);
            Play();
        }

        public void Play()
        {
            Bind();
            KillSequence();

            if (useFade && canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
            }

            if (useScale && animatedRoot != null)
            {
                animatedRoot.localScale = hiddenScale;
            }

            sequence = DOTween.Sequence();
            sequence.SetUpdate(useUnscaledTime);

            if (useFade && canvasGroup != null)
            {
                sequence.Join(canvasGroup.DOFade(1f, Mathf.Max(0.01f, showDuration)));
            }

            if (useScale && animatedRoot != null)
            {
                sequence.Join(animatedRoot.DOScale(visibleScale, Mathf.Max(0.01f, showDuration)).SetEase(showEase));
            }

            if (!useFade && !useScale)
            {
                sequence.AppendInterval(Mathf.Max(0.01f, showDuration));
            }
        }

        public void Hide(Action onComplete)
        {
            Bind();
            KillSequence();

            sequence = DOTween.Sequence();
            sequence.SetUpdate(useUnscaledTime);

            if (useFade && canvasGroup != null)
            {
                sequence.Join(canvasGroup.DOFade(0f, Mathf.Max(0.01f, hideDuration)));
            }

            if (useScale && animatedRoot != null)
            {
                sequence.Join(animatedRoot.DOScale(hiddenScale, Mathf.Max(0.01f, hideDuration)).SetEase(hideEase));
            }

            if (!useFade && !useScale)
            {
                sequence.AppendInterval(Mathf.Max(0.01f, hideDuration));
            }

            sequence.OnComplete(() => onComplete?.Invoke());
        }

        private void Awake()
        {
            Bind();
        }

        private void Reset()
        {
            Bind();
        }

        private void OnValidate()
        {
            Bind();
        }

        private void OnEnable()
        {
            if (playOnEnable)
            {
                Play();
            }
        }

        private void OnDisable()
        {
            KillSequence();

            if (animatedRoot != null)
            {
                animatedRoot.localScale = visibleScale;
            }

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
            }
        }

        private void Bind()
        {
            if (animatedRoot == null)
            {
                animatedRoot = transform;
            }

            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }
        }

        private void KillSequence()
        {
            sequence?.Kill();
            sequence = null;
        }
    }
}
