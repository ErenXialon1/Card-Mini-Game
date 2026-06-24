using CardMiniGame.Wheel;
using UnityEngine;

namespace CardMiniGame.Zones
{
    [CreateAssetMenu(fileName = "zone_config", menuName = "Vertigo Demo/Zone Config")]
    public class ZoneConfig : ScriptableObject
    {
        public int SafeZoneInterval = 5;
        public int SuperZoneInterval = 30;
        public float RewardScalingPerZone = 1.15f;
        public float BombChanceStart = 0.05f;
        public float BombChanceIncreasePerZone = 0.015f;
        public float BombChanceMax = 0.3f;
        public int ContinueBaseCost = 25;
        public int ContinueCostPerZone = 5;
        public WheelConfig NormalWheel;
        public WheelConfig SafeWheel;
        public WheelConfig SuperWheel;
    }
}
