using UnityEngine;
using UnityEngine.UI;

namespace CardMiniGame.UI
{
    [RequireComponent(typeof(Button))]
    public class AutoButtonBinder : MonoBehaviour
    {
        [SerializeField] private Button button;

        public Button Button
        {
            get
            {
                if (button == null)
                {
                    BindButton();
                }

                return button;
            }
        }

        private void Reset()
        {
            BindButton();
            WarnIfInspectorClickIsUsed();
        }

        private void Awake()
        {
            BindButton();
        }

        private void OnValidate()
        {
            BindButton();
            WarnIfInspectorClickIsUsed();
        }

        private void BindButton()
        {
            button = GetComponent<Button>();
        }

        private void WarnIfInspectorClickIsUsed()
        {
            if (button == null || button.onClick.GetPersistentEventCount() == 0)
            {
                return;
            }

            Debug.LogWarning("Button OnClick should be empty. Runtime listeners are registered from code.", this);
        }
    }
}
