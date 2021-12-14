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
        [Header("Object References")]
        //All lists public to be edited in editor
        [SerializeField]
        private TextMeshProUGUI displayText = null;
        [SerializeField]
        private GameObject buttonsContainer = null;

        [Header("Customise")]
        [SerializeField]
        private List<WordColour> wordColours = null;

        public int NumberOfTests { get { return numberOfTests; } set { numberOfTests = Mathf.Clamp(value, 10, 50); } }
        [Header("Option Settings")]

        [SerializeField]
        [Range(10, 50)]
        private int numberOfTests = 20;
        public bool IsRandom { get { return isRandom; } set { isRandom = value; } }
        [SerializeField]
        private bool isRandom = false;

        private List<Button> answerButtons = null;
        private Gui.GuiManager guiManager = null;
        private Results testResults = new Results();
        private int currentTest = 0;
        private float startTime = 0f;

        //Sets up local data and shows warning if issue
        void Start()
        {
            guiManager = GetComponent<Gui.GuiManager>();
            answerButtons = new List<Button>(buttonsContainer.GetComponentsInChildren<Button>());
            if (wordColours.Count < answerButtons.Count)
                Debug.LogError("Not enough colour for amount of buttons. Game not started");
        }

        //Quick function when player gets answers a question
        void Answer(bool isSuccessful)
        {
            RecordResults(isSuccessful);
            RandomiseGame();
        }

        //Randomises game for stroop test
        public void RandomiseGame(bool isFirstTime = false)
        {
            //Igonores when function is used for first time
            if (isFirstTime)
            {
                ResetsGame();
            }
            
            if (currentTest != NumberOfTests)
            {
                List<int> colourSelection = SetColourSelection();
                List<int> buttonSelection = SetButtonOrder(colourSelection);
                SetupGameGui(buttonSelection, colourSelection);
                currentTest++;
            }
            else
            {
                EndGame();
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

        //Records results of each test into a list
        void RecordResults(bool isSuccessful)
        {
            if(isSuccessful)
            {
                testResults.successes++;
            }
        }

        //Toggle event for when toggle for random is changed
        public void SetRandom(bool isSetRandom)
        {
            isRandom = isSetRandom;
        }

        //Input text event for when number of tests change
        public void SetNumberOfTest(string textfield)
        {
            NumberOfTests = System.Int32.Parse(textfield);
        }

        //Resets values, clear events on buttons and restarts start time
        void ResetsGame()
        {
            startTime = Time.time;
            testResults.testTime = 0;
            testResults.successes = 0;
            currentTest = 0;
            foreach (Button button in answerButtons)
                button.onClick.RemoveAllListeners();
        }

        //Ends game by saving final time and displays results
        void EndGame()
        {
            testResults.testTime = Time.time - startTime;
            guiManager.DisplayResults(testResults);
        }

        //Sets colour selection based on being random or standard
        List<int> SetColourSelection()
        {
            if (isRandom)
            {
                return RandomIntSelection(wordColours.Count, 4);
            }
            else
            {
                return RandomIntSelection(4, 4);
            }
        }

        //Sets button orders based on being random or standard
        List<int> SetButtonOrder(List<int> matchSelection)
        {
            if (isRandom)
            {
                return RandomIntSelection(answerButtons.Count, 4);
            }
            else
            {
                return matchSelection;
            }
        }

        //Sets up GUI for game with listeners and correct displayed data
        void SetupGameGui(List<int> buttonOrder, List<int> selectedColours)
        {
            foreach (Button button in answerButtons)
                button.onClick.RemoveAllListeners();

            //Loops through lists of random selection to set up buttons and display
            for (int i = 0; i < answerButtons.Count; i++)
            {
                int selectedButton = buttonOrder[i];
                int selectedColour = selectedColours[i];

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
        }
    }


}
