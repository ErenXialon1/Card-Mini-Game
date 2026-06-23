using UnityEngine;
using UnityEngine.UI;

namespace CardMiniGame.UI
{
    public class InventoryPopupView : MonoBehaviour
    {
        [SerializeField] private AutoButtonBinder closeButton;
        [SerializeField] private PersistentInventoryView inventoryView;
        [SerializeField] private UIAppearAnimator appearAnimator;
        [SerializeField] private bool hideOnAwake = true;

        private bool isOpening;

        public void Show()
        {
            isOpening = true;
            gameObject.SetActive(true);
            isOpening = false;

            inventoryView?.Refresh();
            appearAnimator?.Show();
        }

        public void Hide()
        {
            if (appearAnimator == null || !gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
                return;
            }

            appearAnimator.Hide(() => gameObject.SetActive(false));
        }

        private void Awake()
        {
            RegisterButtonListener();

            if (hideOnAwake && !isOpening)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            RegisterButtonListener();
        }

        private void OnDisable()
        {
            UnregisterButtonListener();
        }

        private void RegisterButtonListener()
        {
            Button button = closeButton == null ? null : closeButton.Button;

            if (button == null)
            {
                return;
            }

            button.onClick.RemoveListener(Hide);
            button.onClick.AddListener(Hide);
        }

        private void UnregisterButtonListener()
        {
            Button button = closeButton == null ? null : closeButton.Button;

            if (button != null)
            {
                button.onClick.RemoveListener(Hide);
            }
        }
    }
}
