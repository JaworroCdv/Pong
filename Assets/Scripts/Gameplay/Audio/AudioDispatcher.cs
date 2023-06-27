namespace Gameplay.Audio
{
    using UnityEngine;

    public class AudioDispatcher : MonoBehaviour
    {
        public void PlaySound(AudioClipData audioClip)
        {
            AudioSource.PlayClipAtPoint(audioClip.AudioClip, transform.position);
        }
    }
}