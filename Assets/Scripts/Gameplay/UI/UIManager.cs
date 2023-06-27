namespace Gameplay.UI
{
    using System.Linq;
    using DG.Tweening;
    using TMPro;
    using UnityEngine;

    public interface IUIManager : ISubManager
    {
        void UpdateScoreBoard();
        void ShowEndGame();
        void ToggleStartGameText(bool isActive);
    }
    
    public class UIManager : MonoBehaviour, IUIManager
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI startGameText;
        
        private IGameContext gameContext;
        
        public void Initialize(IGameContext gameContext)
        {
            this.gameContext = gameContext;
        }

        public void PostInitialize()
        {
            ToggleStartGameText(true);
            
            gameContext.ScoreManager.OnScoreAdded += UpdateScoreBoard;
            gameContext.GameplayManager.OnGameplayEnd += ShowEndGame;
            gameContext.GameplayManager.OnGameplayStart += () => ToggleStartGameText(false);
            gameContext.GameplayManager.OnGameplayRestart += () => ToggleStartGameText(true);
        }

        public void UpdateManager()
        {
        }

        public void UpdateScoreBoard()
        {
            var players = gameContext.PlayersManager.Players.ToList();
            scoreText.text = $"{players[0].Points} : {players[1].Points}";
        }
        
        public void ShowEndGame()
        {
            var players = gameContext.PlayersManager.Players.ToList();
            scoreText.text = $"{players[0].Points} : {players[1].Points}\n{(players[0].Points > players[1].Points ? "Blue Player" : "Red Player")} wins!";
        }

        public void ToggleStartGameText(bool isActive)
        {
            startGameText.gameObject.SetActive(isActive);

            if (isActive)
            {
                DOTween.Sequence()
                    .Append(startGameText.transform.DOScale(1.2f, 1f).From(0.8f))
                    .Append(startGameText.transform.DOScale(0.8f, 1f).From(1.2f))
                    .SetLoops(-1)
                    .SetUpdate(true);
            }
        }
    }
}