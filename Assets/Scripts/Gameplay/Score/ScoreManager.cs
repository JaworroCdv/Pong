namespace Gameplay.Score
{
    using System;
    using Player;
    using UnityEngine;

    public interface IScoreManager : ISubManager
    {
        event Action OnScoreAdded;
        
        void AddScore(PlayerController playerController);
    }
    
    public class ScoreManager : MonoBehaviour, IScoreManager
    {
        public event Action OnScoreAdded;
        
        private IGameContext gameContext;
        
        private const int MaxScore = 5;
        
        public void Initialize(IGameContext gameContext)
        {
            this.gameContext = gameContext;
        }

        public void PostInitialize()
        {
            gameContext.UIManager.UpdateScoreBoard();
        }

        public void UpdateManager()
        {
        }

        public void AddScore(PlayerController playerController)
        {
            playerController.Points++;
            OnScoreAdded?.Invoke();

            if (playerController.Points >= MaxScore)
            {
                gameContext.GameplayManager.EndGameplay();
                return;
            }
            
            gameContext.GameplayManager.RestartGameplay();
        }
    }
}