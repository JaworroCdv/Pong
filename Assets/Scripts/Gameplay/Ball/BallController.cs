using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Ball
{
    public class BallController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rigidbody;
        [SerializeField] private float speed;

        private void Start()
        {
            var randomY = Mathf.Sign(Random.Range(-1, 1));
            var randomX = Mathf.Sign(Random.Range(-1, 1));

            rigidbody.velocity = new Vector2(randomX, randomY) * speed * Time.deltaTime;
        }
    }
}
