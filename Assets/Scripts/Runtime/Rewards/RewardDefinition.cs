using UnityEngine;

namespace CardMiniGame.Rewards
{
    [CreateAssetMenu(fileName = "reward_definition", menuName = "Vertigo Demo/Rewards/Reward Definition")]
    public class RewardDefinition : ScriptableObject
    {
        public string RewardId;
        public string DisplayName;
        public Sprite Icon;
        public int BaseAmount = 1;
        public RewardType RewardType = RewardType.Currency;
        public RewardRarity Rarity = RewardRarity.Common;
    }
}
