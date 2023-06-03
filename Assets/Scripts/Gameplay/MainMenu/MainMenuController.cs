namespace Gameplay.MainMenu
{
    using Constants;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;
    
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Button singlePlayerStart;
        [SerializeField] private Button multiPlayerStart;
        [SerializeField] private Button exitButton;
    
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            singlePlayerStart.onClick.AddListener(StartSinglePlayerGame);
            multiPlayerStart.onClick.AddListener(StartMultiPlayerGame);
            exitButton.onClick.AddListener(ExitGame);
        }

        private void StartSinglePlayerGame()
        {
            SceneManager.LoadSceneAsync(SceneConstants.Gameplay);
        }

        private void StartMultiPlayerGame()
        {
        
        }

        private void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
