using System.Collections.Generic;
using UnityEngine;

namespace CardMiniGame.Wheel
{
    [CreateAssetMenu(fileName = "wheel_config", menuName = "Vertigo Demo/Wheel Config")]
    public class WheelConfig : ScriptableObject
    {
        public WheelType WheelType = WheelType.Normal;
        public List<WheelSliceDefinition> Slices = new List<WheelSliceDefinition>();
        public Sprite WheelBaseSprite;
        public Sprite PointerSprite;
        public SpinVisualProfile VisualProfile;
    }
}
