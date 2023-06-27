namespace Gameplay.Audio
{
    using UnityEngine;

    [CreateAssetMenu]
    public class AudioClipData : ScriptableObject
    {
        [SerializeField] private AudioClip audioClipData;
        
        public AudioClip AudioClip => audioClipData;
    }
}