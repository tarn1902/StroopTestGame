using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Structs;
namespace Game
{
    //Manages the Stroop test gameplay with a basic reset
    public class GameManager : MonoBehaviour
    {
        //All lists public to be edited in editor
        public TextMeshProUGUI displayText = null;
        public List<Button> answerButtons = null;
        public List<WordColour> wordColours = null;
        public List<Results> testResults = null;
        public int maxTests = 10;

        private int currentTest = 0;
        private float startTime = 0f;
        private bool isFirstTime = true;

        //Sets up game for first time if able by reseting game while skipping unneeded tasks
        void Start()
        {
            testResults = new List<Results>();
            //Logs error if unable to display enough unique colours on buttons
            if (wordColours.Count < answerButtons.Count)
                Debug.LogError("Not enough colour for amount of butons. Game not started");
        }

        //Quick funtion when player gets answers a question
        void Answer(bool isSuccessful)
        {
            Debug.Log(isSuccessful);
            RecordResults(isSuccessful);
            ResetGame();
        }


        //Resets the game by randomising colours and button selection to be used in scene
        public void ResetGame()
        {
            //Igonores when function is used for first time
            if (!isFirstTime)
                foreach (Button button in answerButtons)
                    button.onClick.RemoveAllListeners();
            else
                isFirstTime = false;

            if (currentTest == maxTests)
            {
                GuiManager.DisplayResults(testResults);
                testResults.Clear();
                currentTest = 0;
            }
                

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
                    answerButtons[selectedButton].onClick.AddListener(() => Answer(true));
                    answerButtons[selectedButton].GetComponentInChildren<TextMeshProUGUI>().text = wordColours[selectedColour].word;
                    displayText.color = wordColours[selectedColour].colour;
                }
                //Sets up everything else using indexed data
                else
                {
                    //Sets up display with second index data from list
                    if (i == 1)
                        displayText.text = wordColours[selectedColour].word;
                    answerButtons[selectedButton].onClick.AddListener(() => Answer(false));
                    answerButtons[selectedButton].GetComponentInChildren<TextMeshProUGUI>().text = wordColours[selectedColour].word;
                }
            }
            startTime = Time.time;
            currentTest++;

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

        //Records results of each test into a list
        void RecordResults(bool isSuccessful)
        {
            testResults.Add(new Results() { isSuccessful = isSuccessful, testTime = Time.time - startTime });
        }
    }
}
