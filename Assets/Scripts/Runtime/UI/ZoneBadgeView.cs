using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMiniGame.UI
{
    public class ZoneBadgeView : MonoBehaviour
    // Displays the next safe or super zone as a badge.
    {
        [SerializeField] private TMP_Text superZoneText;
        [SerializeField] private TMP_Text safeZoneText;
        [SerializeField] string superZoneLabel = "SUPER ZONE";
        [SerializeField] string safeZoneLabel = "SAFE ZONE";
        [SerializeField] private Image superZoneBadgeImage;
        [SerializeField] private Image safeZoneBadgeImage;

        public void Refresh(int currentZone, int safeZoneInterval, int superZoneInterval)
        {
            int nextSuperZone = GetNextZone(currentZone, superZoneInterval);
            int nextSafeZone = GetNextZone(currentZone, safeZoneInterval);
            superZoneText.text = FormatZoneLabel(superZoneLabel, nextSuperZone);
            safeZoneText.text = FormatZoneLabel(safeZoneLabel, nextSafeZone);
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

        private static string FormatZoneLabel(string label, int zone)
        {
            if (string.IsNullOrEmpty(label))
            {
                return zone.ToString();
            }

            return label.TrimEnd() + " " + zone;
        }
    }
}
