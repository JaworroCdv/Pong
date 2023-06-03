using System.Linq;
using Core.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private Collider2D collider;

        private IGameContext gameContext;
        private int playerIndex;
        private float moveValue;

        private const float ScreenXPadding = 20f;

        public void Initialize(IGameContext gameContext)
        {
            this.gameContext = gameContext;

            playerIndex = gameContext.PlayersManager.Players.ToList().IndexOf(this);
            
            InitializePosition();
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

        private void Update()
        {
            Move();
            HandleViewportPosition();
        }

        private void Move()
        {
            transform.position += Vector3.up * moveSpeed * moveValue * Time.deltaTime;
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
            moveValue = value.Get<float>();
        }
    }
}