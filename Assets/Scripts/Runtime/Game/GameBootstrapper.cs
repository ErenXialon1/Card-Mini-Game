using CardMiniGame.Wheel;
using CardMiniGame.Zones;
using UnityEngine;

namespace CardMiniGame.Game
{
    // Sets up the required systems and starts the initial game flow.
    public class GameBootstrapper : MonoBehaviour
    {
        [SerializeField] private ZoneConfig zoneConfig;
        [SerializeField] private GameSessionController gameSessionController;

        private void Awake()
        {
            if (gameSessionController == null)
            {
                Debug.LogWarning("GameSessionController reference is missing.", this);
                return;
            }

            GameSession session = new GameSession();
            ZoneService zoneService = new ZoneService(zoneConfig);
            WheelSpinResolver spinResolver = new WheelSpinResolver();

            gameSessionController.Initialize(zoneConfig, session, zoneService, spinResolver);
        }
    }
}
