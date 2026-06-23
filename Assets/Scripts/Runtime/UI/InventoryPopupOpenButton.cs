using UnityEngine;
using UnityEngine.UI;

namespace CardMiniGame.UI
{
    [RequireComponent(typeof(AutoButtonBinder))]
    public class InventoryPopupOpenButton : MonoBehaviour
    {
        [SerializeField] private AutoButtonBinder buttonBinder;
        [SerializeField] private InventoryPopupView popupView;

        private void Awake()
        {
            BindButton();
        }

        private void OnEnable()
        {
            RegisterButtonListener();
        }

        private void OnDisable()
        {
            UnregisterButtonListener();
        }

        private void Reset()
        {
            BindButton();
        }

        private void OnValidate()
        {
            BindButton();
        }

        private void OpenPopup()
        {
            if (popupView == null)
            {
                Debug.LogWarning("InventoryPopupView reference is missing.", this);
                return;
            }

            popupView.Show();
        }

        private void RegisterButtonListener()
        {
            BindButton();
            Button button = buttonBinder == null ? null : buttonBinder.Button;

            if (button == null)
            {
                return;
            }

            button.onClick.RemoveListener(OpenPopup);
            button.onClick.AddListener(OpenPopup);
        }

        private void UnregisterButtonListener()
        {
            Button button = buttonBinder == null ? null : buttonBinder.Button;

            if (button != null)
            {
                button.onClick.RemoveListener(OpenPopup);
            }
        }

        private void BindButton()
        {
            if (buttonBinder == null)
            {
                buttonBinder = GetComponent<AutoButtonBinder>();
            }
        }
    }
}
