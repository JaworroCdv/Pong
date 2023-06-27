using UnityEngine;

namespace Gameplay.Player
{
    using Ball;
    using Score;
    using UI;

    public class GameContext : IGameContext
    {
        public GameMode GameMode { get; set; }
        public Camera GameCamera { get; set; }
        public BallController BallController { get; set; }
        public IGameplayManager GameplayManager { get; set; }
        public IPlayersManager PlayersManager { get; set; }
        public IScoreManager ScoreManager { get; set; }
        public IUIManager UIManager { get; set; }
    }
}