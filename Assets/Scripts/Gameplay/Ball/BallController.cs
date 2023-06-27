using UnityEngine;

namespace Gameplay.Ball
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Audio;
    using Constants;
    using Utilities;
    using Random = Random;

    public class BallController : MonoBehaviour, IViewportObject
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D rigidbody;
        [SerializeField] private CircleCollider2D collider;
        [SerializeField] private TrailRenderer trailRenderer;
        [SerializeField] private ParticleSystem particleSystem;
        [SerializeField] private AudioDispatcher audioDispatcher;
        
        [Header("SFX")]
        [SerializeField] private List<AudioClipData> bounceAudioClipDataList;
        [SerializeField] private AudioClipData deathAudioClipData;
        
        [Header("Settings")]
        [SerializeField] private float speed;
        [SerializeField] private float acceleration;

        private IGameContext gameContext;
        private float currentSpeed;
        private Vector2 velocity;
        private bool isInViewport;

        private static readonly Vector2 MaxRandomOffset = new Vector2(0.3f, 0.3f);

        public void Initialize(IGameContext gameContext)
        {
            this.gameContext = gameContext;
            
            Restart();
            
            gameContext.GameplayManager.OnGameplayRestart += Restart;
            gameContext.GameplayManager.OnGameplayStart += () => StartCoroutine(HandleRandomVelocity());
        }

        public void FixedUpdateController()
        {
            if (!isInViewport)
                return;
            
            HandleAcceleration();
            HandleViewportPosition();
        }
        
        private IEnumerator HandleRandomVelocity()
        {
            yield return null;
            yield return new WaitForEndOfFrame();
            
            var randomY = Mathf.Sign(Random.Range(-1, 1));
            var randomX = Mathf.Sign(Random.Range(-1, 1));
                
            var force = new Vector2(randomX, randomY) + GetRandomForceOffset();
            rigidbody.AddForce(force * speed * Time.deltaTime);
        }

        private void Restart()
        {
            transform.position = Vector3.zero;
            rigidbody.velocity = Vector2.zero;
            currentSpeed = speed;
            trailRenderer.Clear();
            
            isInViewport = true;
        }

        public void HandleViewportPosition()
        {
            var cam = gameContext.GameCamera;
            var screenPosition = cam.WorldToScreenPoint(transform.position);

            var (screenMin, screenMax) = GetMinMaxSize();
            var ballScreenHeight = screenMax.y - screenMin.y;
            var ballScreenWidth = screenMax.x - screenMin.x;

            if (screenPosition.y - ballScreenHeight / 2f < 0f || screenPosition.y + ballScreenHeight / 2f > Screen.height)
            {
                rigidbody.velocity = new Vector2(velocity.x, -velocity.y) + GetRandomForceOffset();
            }
            
            if(screenPosition.x - ballScreenWidth / 2f < 0f || screenPosition.x + ballScreenWidth / 2f > Screen.width)
            {
                isInViewport = false;
                
                var rotationX = screenPosition.x < Screen.width / 2f ? 0f : 180f;
                var rotation = Quaternion.Euler(0f, rotationX, 0f);
                Instantiate(particleSystem, transform.position, rotation);
                
                audioDispatcher.PlaySound(deathAudioClipData);
                
                StartCoroutine(WaitAndRestart());
            }
            
            IEnumerator WaitAndRestart()
            {
                yield return new WaitForSeconds(0.6f);
                
                var lostPlayer = gameContext.PlayersManager.GetClosestPlayerByScreenPosition(screenPosition);
                var playerController = gameContext.PlayersManager.Players.First(x => x != lostPlayer);
                gameContext.ScoreManager.AddScore(playerController);
            }
        }

        public (Vector3, Vector3) GetMinMaxSize()
        {
            var cam = gameContext.GameCamera;
            var bounds = collider.bounds;
            
            var min = bounds.min;
            var max = bounds.max;
 
            var screenMin = cam.WorldToScreenPoint(min);
            var screenMax = cam.WorldToScreenPoint(max);
            
            return (screenMin, screenMax);
        }

        private void HandleAcceleration()
        {
            currentSpeed += acceleration * Time.deltaTime;
            rigidbody.velocity = rigidbody.velocity.normalized * currentSpeed;
            velocity = rigidbody.velocity;
        }
        
        private Vector2 GetRandomForceOffset()
        {
            var randomX = Random.Range(-MaxRandomOffset.x, MaxRandomOffset.x);
            var randomY = Random.Range(-MaxRandomOffset.y, MaxRandomOffset.y);

            return new Vector2(randomX, randomY);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag(Tags.Player))
                return;
            
            rigidbody.velocity = Vector2.Reflect(velocity, other.contacts[0].normal) + GetRandomForceOffset();
            audioDispatcher.PlaySound(bounceAudioClipDataList.RandomElement());
        }
    }
}
