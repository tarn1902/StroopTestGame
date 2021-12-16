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

        //Used for buttons to make click sound
        public void PlayClickSound()
        {
            source.PlayOneShot(buttonClickSound);
        }
    }
}

