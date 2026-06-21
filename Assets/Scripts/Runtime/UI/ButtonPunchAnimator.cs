using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CardMiniGame.UI
{
    [RequireComponent(typeof(AutoButtonBinder))]
    public class ButtonPunchAnimator : MonoBehaviour
    {
        [SerializeField] private AutoButtonBinder buttonBinder;
        [SerializeField] private Transform animatedRoot;
        [SerializeField] private Vector3 punchScale = new Vector3(0.08f, 0.08f, 0f);
        [SerializeField] private float duration = 0.16f;
        [SerializeField] private int vibrato = 6;
        [SerializeField] private float elasticity = 0.7f;

        private void Reset()
        {
            BindButton();
        }

        private void Awake()
        {
            BindButton();
        }

        private void OnEnable()
        {
            BindButton();

            Button button = buttonBinder == null ? null : buttonBinder.Button;

            if (button != null)
            {
                button.onClick.AddListener(Play);
            }
        }

        private void OnDisable()
        {
            Button button = buttonBinder == null ? null : buttonBinder.Button;

            if (button != null)
            {
                button.onClick.RemoveListener(Play);
            }
        }

        private void OnValidate()
        {
            BindButton();
        }

        private void Play()
        {
            if (animatedRoot == null)
            {
                return;
            }

            animatedRoot.DOKill();
            animatedRoot.localScale = Vector3.one;
            animatedRoot.DOPunchScale(punchScale, Mathf.Max(0.01f, duration), Mathf.Max(1, vibrato), elasticity);
        }

        private void BindButton()
        {
            buttonBinder = GetComponent<AutoButtonBinder>();
        }
    }
}
