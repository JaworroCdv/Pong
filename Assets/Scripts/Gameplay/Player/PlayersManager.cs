using System.Collections.Generic;
using Core.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Player
{
    public interface IPlayersManager : ISubManager
    {
        IEnumerable<PlayerController> Players { get; }
        IEnumerable<PlayerInitializer> Initializers { get; }
    }
    
    public class PlayersManager : MonoBehaviour, IPlayersManager
    {
        [SerializeField] private PlayerInitializer[] initializers;
        
        public IEnumerable<PlayerController> Players => players;
        public IEnumerable<PlayerInitializer> Initializers => initializers;

        private IGameContext gameContext;
        private List<PlayerController> players;

        public void Initialize(IGameContext gameContext)
        {
            this.gameContext = gameContext;

            players = new List<PlayerController>();
            
            foreach (var player in initializers)
            {
                var input = player.Initialize(gameContext);
                InitializePlayer(input);
            }
        }

        public void UpdateManager()
        {
        }

        private void InitializePlayer(PlayerInput input)
        {
            var playerController = input.GetComponent<PlayerController>();
            players.Add(playerController);
            playerController.Initialize(gameContext);
        }
    }
}