using CardMiniGame.Wheel;

namespace CardMiniGame.Zones
{
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
    }
}
