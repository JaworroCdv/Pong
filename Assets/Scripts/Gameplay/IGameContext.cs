using System.Collections.Generic;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay
{
    using Ball;
    using Score;
    using UI;

    public interface IGameContext
    {
        GameMode GameMode { get; }
        Camera GameCamera { get; }
        BallController BallController { get; }
        IGameplayManager GameplayManager { get; }
        IPlayersManager PlayersManager { get; }
        IScoreManager ScoreManager { get; }
        IUIManager UIManager { get; }
    }
}