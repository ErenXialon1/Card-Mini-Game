using CardMiniGame.Wheel;
using UnityEngine;

namespace CardMiniGame.Zones
{
    // Calculates zone behavior based on the current zone.
    public class ZoneService
    {
        private readonly ZoneConfig config;

        public ZoneService(ZoneConfig config)
        {
            this.config = config;
        }

        public WheelType GetZoneType(int zone)
        {
            if (config != null && config.SuperZoneInterval > 0 && zone % config.SuperZoneInterval == 0)
            {
                return WheelType.Super;
            }

            if (config != null && config.SafeZoneInterval > 0 && zone % config.SafeZoneInterval == 0)
            {
                return WheelType.Safe;
            }

            return WheelType.Normal;
        }

        public bool IsLeaveAllowed(int zone, bool isSpinning)
        {
            if (isSpinning)
            {
                return false;
            }

            WheelType zoneType = GetZoneType(zone);
            return zoneType == WheelType.Safe || zoneType == WheelType.Super;
        }

        public float GetBombChance(int zone)
        {
            if (config == null || GetZoneType(zone) != WheelType.Normal)
            {
                return 0f;
            }

            int safeZone = Mathf.Max(1, zone);
            float startChance = Mathf.Max(0f, config.BombChanceStart);
            float increasePerZone = Mathf.Max(0f, config.BombChanceIncreasePerZone);
            float maxChance = Mathf.Max(startChance, config.BombChanceMax);
            float chance = startChance + (safeZone - 1) * increasePerZone;
            return Mathf.Clamp(chance, startChance, maxChance);
        }

        public int GetContinueCost(int zone)
        {
            if (config == null)
            {
                return 0;
            }

            int safeZone = Mathf.Max(1, zone);
            int baseCost = Mathf.Max(0, config.ContinueBaseCost);
            int costPerZone = Mathf.Max(0, config.ContinueCostPerZone);
            return baseCost + (safeZone - 1) * costPerZone;
        }
    }
}
