using System.Collections.Generic;
using UnityEngine;
using Structs;
namespace Game
{
    //Manages the Stroop test gameplay with a basic reset
    public class GameManager : MonoBehaviour
    {
        public List<WordColour> WordColour { get { return wordColours; } set { wordColours = value; } }
        [Header("Customise")]
        [SerializeField]
        private List<WordColour> wordColours = null;
        public int NumberOfTests { get { return numberOfTests; } set { numberOfTests = Mathf.Clamp(value, 10, 50); } }
        [Header("Option Settings")]
        [SerializeField]
        [Range(10, 50)]
        private int numberOfTests = 20;
        public bool IsRandomisedButtons { get { return isRandomisedButtons; } set { isRandomisedButtons = value; } }
        [SerializeField]
        private bool isRandomisedButtons = false;

        private Gui.GuiManager guiManager = null;
        private Results testResults = new Results();
        private int currentTest = 0;
        private float startTime = 0f;
        private const int numberOfButtons = 4;

        //Sets up local data and shows warning if issue
        void Start()
        {
            guiManager = GetComponent<Gui.GuiManager>();
        }

        //Randomises game for stroop test
        public void RandomiseGame()
        {
            if (currentTest++ != NumberOfTests)
            {
                List<int> colourSelection = isRandomisedButtons ? SetRandomisedColourSelection() : RandomiseIntSelection(numberOfButtons, numberOfButtons);
                List<int> buttonTaskSelection = isRandomisedButtons ? SetRandomisedButtonTasks() : colourSelection;
                guiManager.SetupGameGui(buttonTaskSelection, colourSelection);
            }
            else
            {
                EndGame();
            }
        }

        //Creates list of random intergers based on number range from zero that never repeat
        List<int> RandomiseIntSelection(int size, int selections)
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
        public void RecordSuccess()
        {
            testResults.successes++;
        }

        //Toggle event for when toggle for random is changed
        public void SetRandom(bool isRandom)
        {
            isRandomisedButtons = isRandom;
        }

        //Input text event for when number of tests change
        public void SetNumberOfTest(string textfield)
        {
            NumberOfTests = System.Int32.Parse(textfield);
        }

        //Resets values, clear events on buttons and restarts start time
        public void ResetGame()
        {
            startTime = Time.time;
            testResults.testTime = 0;
            testResults.successes = 0;
            currentTest = 0;
        }

        //Ends game by saving final time and displays results
        void EndGame()
        {
            testResults.testTime = Time.time - startTime;
            guiManager.DisplayResults(testResults);
        }

        //Sets colour selection based on being random or standard
        List<int> SetRandomisedColourSelection()
        {
            return RandomiseIntSelection(wordColours.Count, numberOfButtons);
        }

        //Sets button orders based on being random or standard
        List<int> SetRandomisedButtonTasks()
        {
            return RandomiseIntSelection(numberOfButtons, numberOfButtons);
        }
    }
}
