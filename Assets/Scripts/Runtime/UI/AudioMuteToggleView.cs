using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMiniGame.UI
{
    [RequireComponent(typeof(AutoButtonBinder))]
    public class AudioMuteToggleView : MonoBehaviour
    {
        private const string PlayerPrefsKey = "CardMiniGame.AudioMuted";

        [SerializeField] private AutoButtonBinder buttonBinder;
        [SerializeField] private TMP_Text labelText;
        [SerializeField] private string soundOnLabel = "SFX ON";
        [SerializeField] private string soundOffLabel = "SFX OFF";

        private bool isMuted;

        private void Awake()
        {
            BindButton();
            isMuted = PlayerPrefs.GetInt(PlayerPrefsKey, 0) == 1;
            ApplyMuteState();
            RefreshLabel();
        }

        private void OnEnable()
        {
            RegisterButtonListener();
            RefreshLabel();
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
            RefreshLabel();
        }

        private void ToggleMute()
        {
            isMuted = !isMuted;
            PlayerPrefs.SetInt(PlayerPrefsKey, isMuted ? 1 : 0);
            PlayerPrefs.Save();

            ApplyMuteState();
            RefreshLabel();
        }

        private void RegisterButtonListener()
        {
            BindButton();
            Button button = buttonBinder == null ? null : buttonBinder.Button;

            if (button == null)
            {
                return;
            }

            button.onClick.RemoveListener(ToggleMute);
            button.onClick.AddListener(ToggleMute);
        }

        private void UnregisterButtonListener()
        {
            Button button = buttonBinder == null ? null : buttonBinder.Button;

            if (button != null)
            {
                button.onClick.RemoveListener(ToggleMute);
            }
        }

        private void ApplyMuteState()
        {
            RuntimeManager.MuteAllEvents(isMuted);
        }

        private void RefreshLabel()
        {
            if (labelText != null)
            {
                labelText.text = isMuted ? soundOffLabel : soundOnLabel;
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
