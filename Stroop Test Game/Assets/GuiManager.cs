using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structs;
using TMPro;

public class GuiManager : MonoBehaviour
{
    public GameObject resultsDisplayRef = null;
    public GameObject startDisplayRef = null;
    public GameObject gameDisplayRef = null;
    public TextMeshProUGUI resultsTextRef = null;
    public Game.GameManager manager = null;

    GameObject startDisplay = null;

    static GameObject resultsDisplay = null;
    static GameObject gameDisplay = null;
    static TextMeshProUGUI resultsText = null;


    private void Start()
    {
        resultsDisplay = resultsDisplayRef;
        startDisplay = startDisplayRef;
        gameDisplay = gameDisplayRef;
        resultsText = resultsTextRef;
    }
    public static void DisplayResults(List<Results> results)
    {
        resultsDisplay.SetActive(true);
        gameDisplay.SetActive(false);
        string resultsToText = "";
        int testNum = 0;
        foreach (Results result in results)
        {
            testNum++;
            resultsToText += "Test " + testNum + ": " + (result.isSuccessful ? "Correct" : "Wrong") + " in " + result.testTime + " seconds\n";
        }
        resultsText.text = resultsToText;
    }

    public void DisplayStartMenu()
    {
        resultsDisplay.SetActive(false);
        startDisplay.SetActive(true);
    }

    public void DisplayGame()
    {
        startDisplay.SetActive(false);
        gameDisplay.SetActive(true);
        resultsDisplay.SetActive(false);

        manager.ResetGame();
    }
}
