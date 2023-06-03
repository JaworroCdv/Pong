using UnityEngine;

namespace Gameplay.Player
{
    public class GameContext : IGameContext
    {
        public Camera GameCamera { get; set; }
        public IPlayersManager PlayersManager { get; set; }
    }
}