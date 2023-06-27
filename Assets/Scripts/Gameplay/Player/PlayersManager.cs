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
        PlayerController GetClosestPlayerByScreenPosition(Vector3 screenPosition);
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
            
            var isMultiplayer = this.gameContext.GameMode == GameMode.MultiPlayer;

            foreach (var player in initializers)
            {
                var input = player.Initialize(gameContext);
                InitializePlayer(input, isMultiplayer);
            }
        }

        public void PostInitialize()
        {
            gameContext.GameplayManager.OnGameplayRestart += Restart;
        }

        public void UpdateManager()
        {
            foreach (var player in players)
            {
                player.UpdateController();
            }
        }

        public void Restart()
        {
            foreach (var player in players)
            {
                player.Restart();
            }
        }

        public PlayerController GetClosestPlayerByScreenPosition(Vector3 screenPosition)
        {
            return screenPosition.x < Screen.width / 2f ? players[0] : players[1];
        }

        private void InitializePlayer(PlayerInput input, bool isMultiplayer = false)
        {
            var playerController = input.GetComponent<PlayerController>();
            players.Add(playerController);
            playerController.Initialize(gameContext, isMultiplayer);
        }
    }
}