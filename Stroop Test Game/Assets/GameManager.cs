using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI displayText = null;
    public List<Button> buttons = null;

    

    [System.Serializable]
    public struct WordColour
    {
        public string word;
        public Color colour;
    }

    public List<WordColour> wordColours = null;
    List<WordColour> tempWordColours = new List<WordColour>();


    // Start is called before the first frame update
    void Start()
    {
        ResetGame();
    }

    void CorrectAnswer()
    {
        Debug.Log("Correct Answer");
        ResetGame();
    }

    void WrongAnswer()
    {
        Debug.Log("Wrong Answer");
        ResetGame();
    }

    void ResetGame()
    {
        tempWordColours.Clear();
        tempWordColours.InsertRange(0, wordColours);

        foreach(Button button in buttons)
            button.onClick.RemoveAllListeners();

        int selectedColourWord = Random.Range(0, wordColours.Count);
        tempWordColours.RemoveAt(selectedColourWord);

        int randomSelection = Random.Range(0, tempWordColours.Count);
        int correctButtonIndex = Random.Range(0, buttons.Count);

        buttons[correctButtonIndex].onClick.AddListener(CorrectAnswer);
        buttons[correctButtonIndex].GetComponentInChildren<TextMeshProUGUI>().text = wordColours[selectedColourWord].word;

        displayText.color = wordColours[selectedColourWord].colour;
        displayText.text = tempWordColours[randomSelection].word;

        for (int i = 0; i < buttons.Count; i++)
        {
            if (i != correctButtonIndex)
            {
                int randomWrongSelection = Random.Range(0, tempWordColours.Count);
                buttons[i].onClick.AddListener(WrongAnswer);
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = tempWordColours[randomWrongSelection].word;
                tempWordColours.RemoveAt(randomWrongSelection);
            }

        }
    }
}
