using CardMiniGame.Rewards;

namespace CardMiniGame.Game
{
    public class GameSession
    {
        public int CurrentZone { get; private set; } = 1;
        public RewardInventory CollectedRewards { get; } = new RewardInventory();
        public SessionState SessionState { get; set; } = SessionState.Ready;

        public void AddReward(RewardDefinition reward, int amount)
        {
            CollectedRewards.AddReward(reward, amount);
        }

        public void LoseAllRewards()
        {
            CollectedRewards.Clear();
            SessionState = SessionState.Failed;
        }

        public void AdvanceZone()
        {
            CurrentZone++;
        }

        public void Restart()
        {
            CurrentZone = 1;
            CollectedRewards.Clear();
            SessionState = SessionState.Ready;
        }
    }
}
