using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMiniGame.UI
{
    public class ZoneBadgeView : MonoBehaviour
    {
        [SerializeField] private TMP_Text superZoneText;
        [SerializeField] private TMP_Text safeZoneText;
        [SerializeField] private Image superZoneBadgeImage;
        [SerializeField] private Image safeZoneBadgeImage;
        [SerializeField] private Color superZoneColor = new Color(1f, 0.65f, 0.05f, 1f);
        [SerializeField] private Color safeZoneColor = new Color(0.25f, 0.8f, 0.15f, 1f);

        public void Refresh(int currentZone, int safeZoneInterval, int superZoneInterval)
        {
            int nextSuperZone = GetNextZone(currentZone, superZoneInterval);
            int nextSafeZone = GetNextZone(currentZone, safeZoneInterval);

            if (superZoneText != null)
            {
                superZoneText.text = "SUPER\nZONE " + nextSuperZone;
            }

            if (safeZoneText != null)
            {
                safeZoneText.text = "SAFE\nZONE " + nextSafeZone;
            }

            if (superZoneBadgeImage != null)
            {
                superZoneBadgeImage.color = superZoneColor;
            }

            if (safeZoneBadgeImage != null)
            {
                safeZoneBadgeImage.color = safeZoneColor;
            }
        }

        private static int GetNextZone(int currentZone, int interval)
        {
            if (interval <= 0)
            {
                return currentZone;
            }

            int safeCurrentZone = Mathf.Max(1, currentZone);
            int remainder = safeCurrentZone % interval;
            return remainder == 0 ? safeCurrentZone : safeCurrentZone + (interval - remainder);
        }
    }
}
