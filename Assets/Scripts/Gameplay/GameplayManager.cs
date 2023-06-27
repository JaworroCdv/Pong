using System;
using System.Collections.Generic;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay
{
    using Ball;
    using Constants;
    using MainMenu;
    using Score;
    using UI;
    using UnityEngine.SceneManagement;

    public enum GameMode
    {
        SinglePlayer,
        MultiPlayer
    }

    public interface IGameplayManager
    {
        event Action OnGameplayRestart;
        event Action OnGameplayEnd;
        event Action OnGameplayStart;
        
        void RestartGameplay();
        void EndGameplay();
    }

    public interface ISubManager
    {
        void Initialize(IGameContext gameContext);
        void PostInitialize();
        void UpdateManager();
    }
    
    public class GameplayManager : MonoBehaviour, IGameplayManager
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private BallController ballController;

        [Header("Managers")] 
        [SerializeField] private PlayersManager playersManager;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private UIManager uiManager;

        public event Action OnGameplayRestart;
        public event Action OnGameplayEnd;
        public event Action OnGameplayStart;
        
        private IGameContext gameContext;
        private PlayerInputActions input;
        private List<ISubManager> subManagers;
        
        private bool isGameplayEnded;

        private void Awake()
        {
            input = new PlayerInputActions();
        }
        
        private void Start()
        {
            var mainMenuController = FindObjectOfType<MainMenuController>();

            if (mainMenuController == null)
            {
                SceneManager.LoadSceneAsync(SceneConstants.MainMenu);
                return;
            }

            gameContext = new GameContext
            {
                GameMode = mainMenuController.GameMode,
                GameCamera = mainCamera,
                BallController = ballController,
                GameplayManager = this,
                PlayersManager = playersManager,
                ScoreManager = scoreManager,
                UIManager = uiManager
            };

            subManagers = new List<ISubManager>
            {
                playersManager,
                scoreManager,
                uiManager
            };

            foreach (var manager in subManagers)
            {
                manager.Initialize(gameContext);
            }
            
            ballController.Initialize(gameContext);
            
            foreach (var manager in subManagers)
            {
                manager.PostInitialize();
            }
            
            Time.timeScale = 0f;

            input.Gameplay.Restart.performed += _ =>
            {
                if (isGameplayEnded)
                {
                    SceneManager.LoadSceneAsync(SceneConstants.MainMenu);
                    return;
                }
                
                if (Time.timeScale == 0f)
                {
                    Time.timeScale = 1f;
                    OnGameplayStart?.Invoke();
                    return;
                }
                
                RestartGameplay();
            };
        }

        private void Update()
        {
            if (gameContext == null)
                return;
            
            foreach (var manager in subManagers)
            {
                manager.UpdateManager();
            }
        }

        private void FixedUpdate()
        {
            ballController.FixedUpdateController();
        }

        public void RestartGameplay()
        {
            Time.timeScale = 0f;
            OnGameplayRestart?.Invoke();
        }

        public void EndGameplay()
        {
            isGameplayEnded = true;
            OnGameplayEnd?.Invoke();
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