using CardMiniGame.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMiniGame.Popups
{
    public class CashoutPopupView : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text amountText;
        [SerializeField] private AutoButtonBinder restartButton;
        [SerializeField] private UIAppearAnimator appearAnimator;

        public Button RestartButton => restartButton == null ? null : restartButton.Button;

        public void Show(int totalAmount)
        {
            gameObject.SetActive(true);
            appearAnimator?.Show();

            if (titleText != null)
            {
                titleText.text = "CASH OUT";
            }

            if (amountText != null)
            {
                amountText.text = totalAmount.ToString();
            }
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
    }
}
