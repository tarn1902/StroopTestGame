using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structs;
using TMPro;
using UnityEngine.UI;

namespace Gui
{
    public class GuiManager : MonoBehaviour
    {
        [Header("Display References")]
        [SerializeField]
        private GameObject resultsDisplay = null;
        [SerializeField]
        private GameObject startDisplay = null;
        [SerializeField]
        private GameObject gameDisplay = null;
        [SerializeField]
        private GameObject optionsDisplay = null;

        [Header("Game GUI References")]
        [SerializeField]
        private TextMeshProUGUI displayText = null;
        [SerializeField]
        private GameObject buttonsContainer = null;
        [SerializeField]
        private GameObject answerButtonPrefab = null;

        [Header("Option References")]
        [SerializeField]
        private TextMeshProUGUI resultsText = null;
        [SerializeField]
        private Toggle randomToggle = null;
        [SerializeField]
        private TMP_InputField numberOfTestsInput = null;
        [SerializeField]
        private TMP_InputField numberOfButtonsInput = null;

        private Game.GameManager gameManager = null;
        private Audio.AudioManager audioManager = null;
        private GameObject currentDisplay = null;
        private List<Button> answerButtons = new List<Button>();


        //Turns off all displays to make sure no other gui is on and sets default to start menu to turn on
        private void Start()
        {
            gameManager = GetComponent<Game.GameManager>();
            audioManager = GetComponent<Audio.AudioManager>();
            currentDisplay = startDisplay;
            resultsDisplay.SetActive(false);
            startDisplay.SetActive(false);
            gameDisplay.SetActive(false);
            optionsDisplay.SetActive(false);
            currentDisplay.SetActive(true);
        }

        //The results made by the game will be displayed in results display as a text
        public void DisplayResults(Results results)
        {
            ChangeDisplay(resultsDisplay);
            string resultsToText = "";
            resultsToText += results.successes + "/" + gameManager.NumberOfTests + " in " + results.testTime + " seconds\n";
            resultsText.text = resultsToText;
        }

        //Displays start menu display
        public void DisplayStartMenu()
        {
            ChangeDisplay(startDisplay);
        }

        //Displays game display and randomises first game
        public void DisplayGame()
        {
            SetupGameGUIButtons();
            ChangeDisplay(gameDisplay);
            gameManager.ResetGame();
            gameManager.RandomiseGame();
        }

        //Displays option gui and sets it all match current settings
        public void DisplayOptions()
        {
            ChangeDisplay(optionsDisplay);
            MatchCurrentOptions();
        }

        //Quick turn on and off displays function
        void ChangeDisplay(GameObject to)
        {
            currentDisplay.SetActive(false);
            to.SetActive(true);
            currentDisplay = to;
        }

        //Matches options with game manager values
        void MatchCurrentOptions()
        {
            randomToggle.isOn = gameManager.IsRandomisedButtons;
            numberOfTestsInput.text = gameManager.NumberOfTests.ToString();
            numberOfButtonsInput.text = gameManager.NumberOfButton.ToString();
        }

        //Sets up GUI for game with listeners and correct displayed data
        public void SetupGameGui(List<int> colourButtonPositioning, List<int> selectedColours)
        {
            displayText.color = gameManager.WordColour[selectedColours[0]].colour;
            displayText.text = gameManager.WordColour[selectedColours[1]].word;

            //Loops through lists of random selection to set up buttons and display
            for (int i = 0; i < gameManager.NumberOfButton; i++)
            {
                int currentButtonPosition = colourButtonPositioning[i];
                int selectedColour = selectedColours[i];
                answerButtons[currentButtonPosition].onClick.RemoveListener(gameManager.RecordSuccess);
                if (i == 0) answerButtons[currentButtonPosition].onClick.AddListener(gameManager.RecordSuccess);
                else answerButtons[currentButtonPosition].onClick.RemoveListener(gameManager.RecordSuccess);
                answerButtons[currentButtonPosition].GetComponentInChildren<TextMeshProUGUI>().text = gameManager.WordColour[selectedColour].word;   
            }
        }

        //Dynamically set up buttons for game display
        void SetupGameGUIButtons()
        {
            while (buttonsContainer.transform.childCount < gameManager.NumberOfButton)
            {
                Button button = Instantiate(answerButtonPrefab, buttonsContainer.transform, true).GetComponent<Button>();
                button.onClick.AddListener(audioManager.PlayClickSound);
                button.onClick.AddListener(gameManager.RandomiseGame);
                answerButtons.Add(button.GetComponent<Button>());
            }
            for (int i = 0; i < buttonsContainer.transform.childCount; i++)
            {
                if (i <= gameManager.NumberOfButton)
                {
                    buttonsContainer.transform.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    buttonsContainer.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }
}
