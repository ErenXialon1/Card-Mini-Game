using UnityEngine;

namespace CardMiniGame.Wheel
{
    [CreateAssetMenu(fileName = "spin_visual_profile", menuName = "Vertigo Demo/Wheel/Spin Visual Profile")]
    public class SpinVisualProfile : ScriptableObject
    {
        public float SpinDuration = 3f;
        public int MinimumFullRotations = 4;
        public Color WheelTint = Color.white;
        public Color PointerTint = Color.white;
    }
}
