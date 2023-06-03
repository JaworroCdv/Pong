using System.Linq;
using Core.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Player
{
    public class PlayerInitializer : MonoBehaviour
    {
        [SerializeField] private PlayerInput input;
        
        private int playerIndex;
        
        private const string FirstPlayerControlScheme = "Player1";
        private const string SecondPlayerControlScheme = "Player2";

        public PlayerInput Initialize(IGameContext gameContext)
        {
            playerIndex = gameContext.PlayersManager.Initializers.ToList().IndexOf(this);
            
            return AssignProperInputsActions();
        }

        private PlayerInput AssignProperInputsActions()
        {
            var controlScheme = playerIndex == 0
                ? FirstPlayerControlScheme
                : SecondPlayerControlScheme;
            
            return input.InitializePlayer(controlScheme);
        }
    }
}