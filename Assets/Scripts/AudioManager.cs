using UnityEngine;
namespace Audio
{
    /*
     * This class manages how audio is used within scene
     * Tarn Cooper
     */
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

        public bool MuteGameAudio { get { return muteGameAudio; } set { muteGameAudio = value; } }
        private bool muteGameAudio = false;

        //Plays menu music on starting application
        private void Start()
        {
            PlayMenuMusic();
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
            if (!MuteGameAudio && (!source.isPlaying || source.clip != gameMusic))
            {
                source.clip = gameMusic;
                source.Play();
            }
            else
            {
                source.Stop();
            }
        }

        //***Button Events***
        //Used for buttons to make click sound
        public void PlayClickSound()
        {
            source.PlayOneShot(buttonClickSound);
        }
    }
}

