using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CardMiniGame.UI
{
    // Changes UI scale on hover and interaction.
    [DisallowMultipleComponent]
    public class UIScaleHoverEffect : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        ISelectHandler,
        IDeselectHandler,
        IPointerDownHandler,
        IPointerUpHandler
    {
        [SerializeField] private Transform scaleTarget;
        [SerializeField] private bool useCurrentScaleAsNormal = true;
        [SerializeField] private Vector3 normalScale = Vector3.one;
        [SerializeField] private Vector3 activeScale = new Vector3(1.04f, 1.04f, 1f);
        [SerializeField] private bool enablePressScale = true;
        [SerializeField] private Vector3 pressScale = new Vector3(0.98f, 0.98f, 1f);
        [SerializeField] private float duration = 0.12f;
        [SerializeField] private Ease ease = Ease.OutQuad;
        [SerializeField] private bool useUnscaledTime = true;
        [SerializeField] private bool requireInteractableSelectable = true;

        private Selectable selectable;
        private Tween tween;
        private bool isHovered;
        private bool isSelected;
        private bool isPressed;
        private bool isForcedActive;

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovered = true;
            Refresh(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHovered = false;
            isPressed = false;
            Refresh(true);
        }

        public void OnSelect(BaseEventData eventData)
        {
            isSelected = true;
            Refresh(true);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            isSelected = false;
            isPressed = false;
            Refresh(true);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            isPressed = true;
            Refresh(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            isPressed = false;
            Refresh(true);
        }

        public void SetForcedActive(bool active)
        {
            isForcedActive = active;
            Refresh(true);
        }

        private void Awake()
        {
            Bind();

            if (useCurrentScaleAsNormal && scaleTarget != null)
            {
                normalScale = scaleTarget.localScale;
            }

            ApplyScaleInstant(normalScale);
        }

        private void Reset()
        {
            Bind();
        }

        private void OnValidate()
        {
            Bind();
        }

        private void OnDisable()
        {
            tween?.Kill();
            isHovered = false;
            isSelected = false;
            isPressed = false;
            isForcedActive = false;
            ApplyScaleInstant(normalScale);
        }

        private void Refresh(bool animated)
        {
            if (scaleTarget == null)
            {
                return;
            }

            Vector3 targetScale = ResolveTargetScale();

            if (!animated)
            {
                ApplyScaleInstant(targetScale);
                return;
            }

            tween?.Kill();
            tween = scaleTarget
                .DOScale(targetScale, Mathf.Max(0.01f, duration))
                .SetEase(ease)
                .SetUpdate(useUnscaledTime);
        }

        private Vector3 ResolveTargetScale()
        {
            if (!CanApplyActiveState())
            {
                return normalScale;
            }

            if (enablePressScale && isPressed)
            {
                return pressScale;
            }

            return isHovered || isSelected || isForcedActive ? activeScale : normalScale;
        }

        private bool CanApplyActiveState()
        {
            if (!requireInteractableSelectable || selectable == null)
            {
                return true;
            }

            return selectable.IsActive() && selectable.IsInteractable();
        }

        private void ApplyScaleInstant(Vector3 scale)
        {
            if (scaleTarget != null)
            {
                scaleTarget.localScale = scale;
            }
        }

        private void Bind()
        {
            if (scaleTarget == null)
            {
                scaleTarget = transform;
            }

            selectable = GetComponent<Selectable>();
        }
    }
}
