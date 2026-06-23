using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMiniGame.UI
{
    public class ZoneTrackView : MonoBehaviour
    {
        [SerializeField] private List<TMP_Text> zoneTexts = new List<TMP_Text>();
        [SerializeField] private Image currentZoneMarker;
        [SerializeField] private Color normalZoneColor;
        [SerializeField] private Color safeZoneColor;
        [SerializeField] private Color superZoneColor;
        [SerializeField] private Color currentZoneColor = Color.white;

        public void Refresh(int currentZone, int safeZoneInterval, int superZoneInterval)
        {
            if (zoneTexts == null || zoneTexts.Count == 0)
            {
                return;
            }

            int centerIndex = zoneTexts.Count / 2;
            int firstZone = Mathf.Max(1, currentZone - centerIndex);

            for (int i = 0; i < zoneTexts.Count; i++)
            {
                TMP_Text zoneText = zoneTexts[i];

                if (zoneText == null)
                {
                    continue;
                }

                int zone = firstZone + i;
                bool isCurrent = zone == currentZone;
                zoneText.text = zone.ToString();
                zoneText.color = isCurrent
                    ? currentZoneColor
                    : GetZoneColor(zone, safeZoneInterval, superZoneInterval);
            }

            if (currentZoneMarker != null)
            {
                currentZoneMarker.color = GetMarkerColor(currentZone, safeZoneInterval, superZoneInterval);
            }
        }

        private Color GetMarkerColor(int zone, int safeZoneInterval, int superZoneInterval)
        {
            return GetZoneColor(zone, safeZoneInterval, superZoneInterval);
        }

        private Color GetZoneColor(int zone, int safeZoneInterval, int superZoneInterval)
        {
            if (superZoneInterval > 0 && zone % superZoneInterval == 0)
            {
                return superZoneColor;
            }

            if (safeZoneInterval > 0 && zone % safeZoneInterval == 0)
            {
                return safeZoneColor;
            }

            return normalZoneColor;
        }
    }
}
