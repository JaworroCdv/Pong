using System.Collections.Generic;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay
{
    public interface IGameContext
    {
        Camera GameCamera { get; }
        IPlayersManager PlayersManager { get; }
    }
}