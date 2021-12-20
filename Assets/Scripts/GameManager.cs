using System.Collections.Generic;
using System.Collections;
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
        public int NumberOfButton { get { return numberOfButtons; } set { numberOfButtons = Mathf.Clamp(value, 4, 8); } }
        [SerializeField]
        [Range(4,8)]
        private int numberOfButtons = 4;

        private Gui.GuiManager guiManager = null;
        private Results testResults = new Results();
        private int currentTestNumber = 0;
        private float startTime = 0f;

        //Sets up local data and shows warning if issue
        void Start()
        {
            guiManager = GetComponent<Gui.GuiManager>();
        }

        //Randomises game for stroop test
        public void RandomiseGame()
        {
            if (++currentTestNumber > NumberOfTests)
            {
                testResults.testTime = Time.time - startTime;
                StartCoroutine(DelayResultTillEndOfFrame());
                return;
            }

            List<int> colourSelection = isRandomisedButtons ? SetRandomisedColourSelection() : RandomiseIntSelection(numberOfButtons, numberOfButtons);
            List<int> buttonTaskSelection = isRandomisedButtons ? SetRandomisedButtonTasks() : colourSelection;
            guiManager.SetupGameGui(buttonTaskSelection, colourSelection);
            guiManager.SetGameTestNumber(currentTestNumber);
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

        //Input text event for when number of tests changes
        public void SetNumberOfTest(string textfield)
        {
            NumberOfTests = System.Int32.Parse(textfield);
        }

        //Input text event for when number of buttons changes
        public void SetNumberOfButtons(string textfield)
        {
            numberOfButtons = System.Int32.Parse(textfield);
        }

        //Resets values, clear events on buttons and restarts start time
        public void ResetGame()
        {
            startTime = Time.time;
            testResults.testTime = 0;
            testResults.successes = 0;
            currentTestNumber = 0;
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

        //Quits application
        public void QuitGame()
        {
            Application.Quit();
        }

        //Delays Displaying results till final event is sent
        private IEnumerator DelayResultTillEndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            guiManager.DisplayResults(testResults);
        }
    }
}
