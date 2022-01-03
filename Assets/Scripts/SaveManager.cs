using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Save
{
    /*
     * This class manages how saving of data is handled
     * Tarn Cooper
     */
    public class SaveManager : MonoBehaviour
    {
        //Saves local highscore to playerprefs
        public void SetHighScore(int score)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }

        //Sets name for highscore holder
        public void SetName(string name)
        {
            if (name.Trim() == "")
                PlayerPrefs.SetString("Name", "Unknown");
            else
                PlayerPrefs.SetString("Name", name);
        }

        //Gets local highscore from playerprefs
        public int GetHighScore()
        {
            return PlayerPrefs.GetInt("HighScore");
        }

        //Gets local name of highscore holder
        public string GetName()
        {
            return PlayerPrefs.GetString("Name");
        }

        
        //Resets highscore to 0
        private void ClearHighScore()
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }

        //Replaces name with Unknown
        private void ClearName()
        {
            PlayerPrefs.SetString("Name", "Unknown");
        }

        //***Button Events***
        public void ClearData()
        {
            ClearHighScore();
            ClearName();
        }
    }
}
