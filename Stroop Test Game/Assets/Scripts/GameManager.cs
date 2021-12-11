using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Manages the Stroop test gameplay with a basic reset
public class GameManager : MonoBehaviour
{
    //All lists public to be edited in editor
    public TextMeshProUGUI displayText = null;
    public List<Button> answerButtons = null;
    public List<WordColour> wordColours = null;

    //Basic struct to allow to link a word to a colour and be seen in Unity editor
    [System.Serializable]
    public struct WordColour
    {
        public string word;
        public Color colour;
    }

    //Sets up game for first time if able by reseting game while skipping unneeded tasks
    void Start()
    {
        //Logs error if unable to display enough unique colours on buttons
        if (wordColours.Count < answerButtons.Count)
            Debug.LogError("Not enough colour for amount of butons. Game not started");
        else
        {
            ResetGame(true);
        }
    }

    //Quick debug funtion when player gets answer right
    void CorrectAnswer()
    {
        Debug.Log("Correct Answer");
        ResetGame(false);
    }

    //Quick debug funtion when player gets answer wrong
    void WrongAnswer()
    {
        Debug.Log("Wrong Answer");
        ResetGame(false);
    }

    //Resets the game by randomising colours and button selection to be used in scene
    void ResetGame(bool isFirstTime)
    {
        //Igonores when function is used for first time
        if (!isFirstTime)
            foreach (Button button in answerButtons)
                button.onClick.RemoveAllListeners();

        List<int> randomButtonSelection = RandomIntSelection(answerButtons.Count, 4);
        List<int> randomColourSelection = RandomIntSelection(wordColours.Count, 4);

        //Loops through lists of random selection to set up buttons and display
        for (int i = 0; i < answerButtons.Count; i++)
        {
            int selectedButton = randomButtonSelection[i];
            int selectedColour = randomColourSelection[i];

            //Sets up the settings buttons and display from first indexed data from lists
            if (i == 0)
            {
                answerButtons[selectedButton].onClick.AddListener(CorrectAnswer);
                answerButtons[selectedButton].GetComponentInChildren<TextMeshProUGUI>().text = wordColours[selectedColour].word;
                displayText.color = wordColours[selectedColour].colour;
            }
            //Sets up everything else using indexed data
            else
            {
                //Sets up display with second index data from list
                if (i == 1)
                    displayText.text = wordColours[selectedColour].word;
                answerButtons[selectedButton].onClick.AddListener(WrongAnswer);
                answerButtons[selectedButton].GetComponentInChildren<TextMeshProUGUI>().text = wordColours[selectedColour].word;
            }
            
        }
    }

    //Creates list of random intergers based on number range from zero that never repeat
    List<int> RandomIntSelection(int size, int selections)
    {
        List<int> selectableInts = new List<int>();
        List<int> selectedInts = new List<int>();

        //Fills list with all possible numbers in range
        for (int i = 0; i < size; i++)
            selectableInts.Add(i);

        //Selects random numbers while removing them from the possible selection
        while (selectedInts.Count != selections)
        {
            int randomIndex = Random.Range(0, selectableInts.Count);
            selectedInts.Add(selectableInts[randomIndex]);
            selectableInts.RemoveAt(randomIndex);
        }
        return selectedInts;
    }
}
