using CardMiniGame.Rewards;
using CardMiniGame.Wheel;
using FMODUnity;
using UnityEngine;

namespace CardMiniGame.Game
{
    public class GameFeedbackAudio : MonoBehaviour
    {
        [SerializeField] private EventReference bombExplosionEvent;
        [SerializeField] private EventReference bombFailEvent;
        [SerializeField] private EventReference rewardCoinSmallEvent;
        [SerializeField] private EventReference rewardCashCollectEvent;
        [SerializeField] private EventReference rewardPowerupEvent;
        [SerializeField] private EventReference bigRewardEvent;
        [SerializeField] private EventReference safeZoneEnterEvent;
        [SerializeField] private EventReference superZoneEnterEvent;
        [SerializeField] private EventReference normalZoneChangedEvent;

        public void PlayBomb()
        {
            Play(bombExplosionEvent);
            Play(bombFailEvent);
        }

        public void PlayReward(RewardDefinition reward)
        {
            if (reward == null)
            {
                return;
            }

            if (reward.RewardType == RewardType.Bomb)
            {
                PlayBomb();
                return;
            }

            if (reward.Rarity == RewardRarity.Legendary)
            {
                Play(bigRewardEvent);
                return;
            }

            if (reward.RewardType == RewardType.Special || reward.Rarity == RewardRarity.Epic)
            {
                Play(rewardPowerupEvent);
                return;
            }

            if (reward.Rarity == RewardRarity.Rare)
            {
                Play(rewardCashCollectEvent);
                return;
            }

            Play(rewardCoinSmallEvent);
        }

        public void PlayZoneChanged(WheelType zoneType)
        {
            switch (zoneType)
            {
                case WheelType.Safe:
                    Play(safeZoneEnterEvent);
                    break;
                case WheelType.Super:
                    Play(superZoneEnterEvent);
                    break;
                default:
                    Play(normalZoneChangedEvent);
                    break;
            }
        }

        private void Play(EventReference eventReference)
        {
            if (eventReference.IsNull)
            {
                return;
            }

            RuntimeManager.PlayOneShot(eventReference, transform.position);
        }
    }
}
