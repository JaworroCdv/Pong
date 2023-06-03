using System;
using System.Collections.Generic;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay
{
    public interface IGameplayManager
    {
    }

    public interface ISubManager
    {
        void Initialize(IGameContext gameContext);
        void UpdateManager();
    }
    
    public class GameplayManager : MonoBehaviour, IGameplayManager
    {
        [SerializeField] private Camera mainCamera;

        [Header("Managers")] 
        [SerializeField] private PlayersManager playersManager;
        
        private IGameContext gameContext;
        private PlayerInputActions input;
        private List<ISubManager> subManagers;

        private void Awake()
        {
            input = new PlayerInputActions();
        }
        
        private void Start()
        {
            gameContext = new GameContext
            {
                GameCamera = mainCamera,
                PlayersManager = playersManager
            };

            subManagers = new List<ISubManager>
            {
                playersManager
            };

            foreach (var manager in subManagers)
            {
                manager.Initialize(gameContext);
            }
        }

        private void Update()
        {
            foreach (var manager in subManagers)
            {
                manager.UpdateManager();
            }
        }

        private void OnEnable()
        {
            input.Enable();
        }

        private void OnDisable()
        {
            input.Disable();
        }
    }
}