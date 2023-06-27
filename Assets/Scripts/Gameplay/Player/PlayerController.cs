using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private Collider2D collider;

        public Vector2 Position => transform.position;
        public int Points { get; set; }
        
        private IGameContext gameContext;
        
        private int playerIndex;
        private float moveValue;
        private bool isMultiplayer;

        private const float ScreenXPadding = 20f;
        private const float AIMovementMultiplier = 0.7f;

        public void Initialize(IGameContext gameContext, bool isMultiplayer)
        {
            this.gameContext = gameContext;
            this.isMultiplayer = isMultiplayer;

            playerIndex = gameContext.PlayersManager.Players.ToList().IndexOf(this);
            
            InitializePosition();
        }

        public void Restart()
        {
            InitializePosition();
        }

        public void UpdateController()
        {
            Move();
            HandleViewportPosition();
        }

        private void InitializePosition()
        {
            var position = gameContext.GameCamera.WorldToScreenPoint(transform.position);
           
            var (screenMin, screenMax) = GetMinMaxSize();
            var racketScreenWidth = screenMax.x - screenMin.x;
            
            position.x = playerIndex == 0 
                ? ScreenXPadding + racketScreenWidth / 2f
                : Screen.width - ScreenXPadding - racketScreenWidth / 2f;
            
            transform.position = gameContext.GameCamera.ScreenToWorldPoint(position);
        }

        private void Move()
        {
            if(isMultiplayer || (!isMultiplayer && playerIndex == 0))
            {
                transform.position += Vector3.up * moveSpeed * moveValue * Time.deltaTime;
            }
            else if (!isMultiplayer && playerIndex == 1)
            {
                var cam = gameContext.GameCamera;
                var ballPosition = gameContext.BallController.transform.position;
                var ballViewportPositionX = cam.WorldToViewportPoint(ballPosition).x;
                
                if (ballViewportPositionX < 0.5f)
                    return;
                
                var playerPosition = transform.position;
                
                var targetPositionY = Mathf.Lerp(playerPosition.y, ballPosition.y, moveSpeed * Time.deltaTime * AIMovementMultiplier);
                transform.position = new Vector3(playerPosition.x, targetPositionY, playerPosition.z);
            }
        }

        private void HandleViewportPosition()
        {
            var cam = gameContext.GameCamera;
            var screenPosition = cam.WorldToScreenPoint(transform.position);

            var (screenMin, screenMax) = GetMinMaxSize();
            var racketScreenHeight = screenMax.y - screenMin.y;

            if (screenPosition.y - racketScreenHeight / 2f < 0f)
            {
                screenPosition.y = racketScreenHeight / 2f;
            } else if (screenPosition.y + racketScreenHeight / 2f > Screen.height)
            {
                screenPosition.y = Screen.height - racketScreenHeight / 2f;
            }

            transform.position = cam.ScreenToWorldPoint(screenPosition);
        }

        private (Vector3, Vector3) GetMinMaxSize()
        {
            var cam = gameContext.GameCamera;
            var bounds = collider.bounds;
            
            var min = bounds.min;
            var max = bounds.max;
 
            var screenMin = cam.WorldToScreenPoint(min);
            var screenMax = cam.WorldToScreenPoint(max);
            
            return (screenMin, screenMax);
        }

        private void OnMove(InputValue value)
        {
            if (!isMultiplayer && playerIndex != 0)
                return;
            
            moveValue = value.Get<float>();
        }
    }
}