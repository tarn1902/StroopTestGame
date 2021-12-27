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

        [Header("Background References")]
        [SerializeField]
        private Image backgroudImage = null;
        [SerializeField]
        private Sprite[] backgroudImageOptions = null;

        [Header("Game GUI References")]
        [SerializeField]
        private TextMeshProUGUI displayText = null;
        [SerializeField]
        private GameObject buttonsContainer = null;
        [SerializeField]
        private GameObject answerButtonPrefab = null;
        [SerializeField]
        private TextMeshProUGUI testNumberText = null;

        [Header("Option References")]
        [SerializeField]
        private Toggle randomToggle = null;
        [SerializeField]
        private TMP_InputField numberOfTestsInput = null;
        [SerializeField]
        private TMP_InputField numberOfButtonsInput = null;
        [SerializeField]
        private Toggle muteToggle = null;
        [SerializeField]
        private TMP_Dropdown backgroundDropdown = null;

        [Header("Results References")]
        [SerializeField]
        private TextMeshProUGUI resultsText = null;
        [SerializeField]
        private TMP_InputField userNameText = null;

        [Header("Score Tuning")]
        [SerializeField]
        private float scoreCompareTime = 1;
        [SerializeField]
        private float scoreMultiplier = 10f;

        private Game.GameManager gameManager = null;
        private Audio.AudioManager audioManager = null;
        private Save.SaveManager saveManager = null;
        private GameObject currentDisplay = null;
        private List<Button> answerButtons = new List<Button>();
        private int backgroundIndex = 1;

        //Turns off all displays to make sure no other gui is on and sets default to start menu to turn on
        private void Start()
        {
            gameManager = GetComponent<Game.GameManager>();
            audioManager = GetComponent<Audio.AudioManager>();
            saveManager = GetComponent<Save.SaveManager>();
            ResetGUI();
        }

        //Resets GUI to correct states to run game
        private void ResetGUI()
        {
            currentDisplay = startDisplay;
            resultsDisplay.SetActive(false);
            gameDisplay.SetActive(false);
            optionsDisplay.SetActive(false);
            ChangeDisplay(startDisplay);
        }

        //Quick turn on and off displays function
        private void ChangeDisplay(GameObject to)
        {
            currentDisplay.SetActive(false);
            to.SetActive(true);
            currentDisplay = to;
        }

        //Sets up GUI for game with listeners and correct displayed data
        public void SetupGameGui(List<int> colourButtonPositioning, List<int> selectedColours)
        {
            displayText.color = gameManager.WordColour[selectedColours[0]].colour;
            displayText.text = gameManager.WordColour[selectedColours[1]].word;

            //Loops through lists of random selection to set up buttons and display
            for (int i = 0; i < gameManager.NumberOfButtons; i++)
            {
                int currentButtonPosition = colourButtonPositioning[i];
                int selectedColour = selectedColours[i];
                answerButtons[currentButtonPosition].onClick.RemoveListener(gameManager.RecordSuccess);
                if (i == 0) answerButtons[currentButtonPosition].onClick.AddListener(gameManager.RecordSuccess);
                answerButtons[currentButtonPosition].GetComponentInChildren<TextMeshProUGUI>().text = gameManager.WordColour[selectedColour].word;
            }
        }

        //Dynamically set up buttons for game display
        void SetupGameGUIButtons()
        {
            while (buttonsContainer.transform.childCount < gameManager.NumberOfButtons)
            {
                Button button = Instantiate(answerButtonPrefab, buttonsContainer.transform, true).GetComponent<Button>();
                button.onClick.AddListener(audioManager.PlayClickSound);
                button.onClick.AddListener(gameManager.RandomiseGame);
                answerButtons.Add(button.GetComponent<Button>());
            }
            for (int i = 0; i < buttonsContainer.transform.childCount; i++)
            {
                if (i <= gameManager.NumberOfButtons)
                {
                    buttonsContainer.transform.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    buttonsContainer.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
        //Sets game text to display test number
        public void SetGameTestNumber(int testNum)
        {
            testNumberText.text = testNum.ToString();
        }

        //***Button Events***
        //The results made by the game will be displayed in results display as a text
        public void DisplayResults(Results results)
        {
            if (saveManager.GetName() == "")
                saveManager.SetName("Unknown");
            ChangeDisplay(resultsDisplay);
            string resultsToText = "";
            int savedHighScore = saveManager.GetHighScore();
            int newScore = (int)(scoreCompareTime / (results.testTime / gameManager.NumberOfTests) * (results.successes / gameManager.NumberOfTests) * scoreMultiplier);
            resultsToText += "Total Correct: " + results.successes + "/" + gameManager.NumberOfTests +
                            "\n Total Time: " + results.testTime + " seconds" +
                            "\n Score: " + newScore +
                            "\n" + (newScore > savedHighScore ? "New High Score!!" : "High Score from " + saveManager.GetName() + ": " + savedHighScore);
            resultsText.text = resultsToText;
            if (newScore > savedHighScore)
            {
                saveManager.SetHighScore(newScore);
                userNameText.text = saveManager.GetName();
                userNameText.gameObject.SetActive(true);
            }
            audioManager.PlayMenuMusic();
        }

        //Displays start menu display
        public void DisplayStartMenu()
        {
            ChangeDisplay(startDisplay);
            audioManager.PlayMenuMusic();
        }

        //Displays game display and randomises first game
        public void DisplayGame()
        {
            SetupGameGUIButtons();
            ChangeDisplay(gameDisplay);
            audioManager.PlayGameMusic();
            gameManager.ResetGame();

        }

        //Displays option gui and sets it all match current settings
        public void DisplayOptions()
        {
            ChangeDisplay(optionsDisplay);
            MatchCurrentOptions();
        }

        //Matches options with game manager values
        public void MatchCurrentOptions()
        {
            randomToggle.isOn = gameManager.IsRandomisedButtons;
            numberOfTestsInput.text = gameManager.NumberOfTests.ToString();
            numberOfButtonsInput.text = gameManager.NumberOfButtons.ToString();
            muteToggle.isOn = audioManager.MuteGameAudio;
            backgroundDropdown.value = backgroundIndex;

        }

        //Sets background based on index of images
        public void SetBackground(int dropIndex)
        {
            backgroundIndex = dropIndex;
            if (backgroundIndex == 0)
                backgroudImage.enabled = false;
            else
            {
                backgroudImage.enabled = true;
                backgroudImage.sprite = backgroudImageOptions[backgroundIndex - 1];
            }
        }
    }
}
