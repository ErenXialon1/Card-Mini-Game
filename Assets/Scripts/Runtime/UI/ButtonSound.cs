using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CardMiniGame.UI
{
    [DisallowMultipleComponent]
    public class ButtonSound : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IPointerUpHandler
    {
        [SerializeField] private EventReference pointerEnterEvent;
        [SerializeField] private EventReference pointerExitEvent;
        [SerializeField] private EventReference pointerDownEvent;
        [SerializeField] private EventReference pointerUpEvent;

        public void OnPointerEnter(PointerEventData eventData)
        {
            Play(pointerEnterEvent);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Play(pointerExitEvent);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            Play(pointerDownEvent);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            Play(pointerUpEvent);
        }

        private void Play(EventReference eventReference)
        {
            if (eventReference.IsNull)
            {
                return;
            }

            RuntimeManager.PlayOneShot(eventReference, transform.position);
        }
    }
}
