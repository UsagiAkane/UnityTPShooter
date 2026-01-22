using UnityEngine;

namespace Managers
{
    public class SFXmanager : MonoBehaviour//TODO Rework this or start use?
    {
        public static SFXmanager instance;

        [SerializeField] private AudioSource sFXObject;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public void PlaySFXClip(AudioClip clip, Transform spawn, float volume)//TODO Rework this or start use?
        {
            //spawn in gameobj
            AudioSource audioSource = Instantiate(sFXObject, spawn.position, Quaternion.identity);
            //assign clip
            audioSource.clip = clip;
            //assign vol
            audioSource.volume = volume;
            //play
            audioSource.Play();
            //get length of clip
            float clipLength = audioSource.clip.length;
            //destroy 
            Destroy(audioSource.gameObject, clipLength);
        }
    }
}