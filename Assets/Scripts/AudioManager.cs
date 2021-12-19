using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        private AudioSource source = null;
        [SerializeField]
        private AudioClip buttonClickSound = null;
        [SerializeField]
        private AudioClip menuMusic = null;
        [SerializeField]
        private AudioClip gameMusic = null;

        //Used for buttons to make click sound
        public void PlayClickSound()
        {
            source.PlayOneShot(buttonClickSound);
        }

        //Plays music when in menu
        public void PlayMenuMusic()
        {
            if (!source.isPlaying || source.clip != menuMusic)
            {
                source.clip = menuMusic;
                source.Play();
            }
        }

        //Plays music when in game
        public void PlayGameMusic()
        {
            if (!source.isPlaying || source.clip != gameMusic)
            {
                source.clip = gameMusic;
                source.Play();
            }
        }
    }
}

