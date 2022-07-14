using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private GameObject readyButton;
    private GameObject throwButton;
    private GameObject saveScoreButton;

    private Text activePlayerText;
    private Text totalScoreText;
    private Text activeScoreText;
    private Text currentScoreText;
    private Text dicesNumberText;

    private Game game;

    private void Awake()
    {
        //Buttons
        readyButton = GameObject.Find("ReadyButton");
        throwButton = GameObject.Find("ThrowButton");
        saveScoreButton = GameObject.Find("SaveScoreButton");

        //Texts
        activePlayerText = GameObject.Find("ActivePlayer").GetComponent<Text>();
        totalScoreText = GameObject.Find("TotalScore").GetComponent<Text>();
        activeScoreText = GameObject.Find("ActiveScore").GetComponent<Text>();
        currentScoreText = GameObject.Find("CurrentScore").GetComponent<Text>();
        dicesNumberText = GameObject.Find("DicesNumber").GetComponent<Text>();

        game = GameObject.Find("Game").GetComponent<Game>();
    }

    private void Start()
    {
        readyButton.SetActive(false);
        throwButton.SetActive(false);
        saveScoreButton.SetActive(false);

        activePlayerText.text = "Wait...";
        totalScoreText.text = "0 :T";
        activeScoreText.text = "0 :A";
        currentScoreText.text = "0 :C";
    }

    public void SetActiveReady(bool value)
    {
        readyButton.SetActive(value);
    }

    public void SetActiveThrow(bool value)
    {
        throwButton.SetActive(value);
    }

    public void SetActiveSave(bool value)
    {
        saveScoreButton.SetActive(value);
    }

    public void Ready()
    {
        game.Ready();
    }

    public void Throw()
    {
        game.Throw();
    }

    public void Save()
    {
        game.Save();
    }

    public void UpdateTextActivePlayer(string text)
    {
        activePlayerText.text = text;
    }

    public void UpdateTextCount(DiceScript[] dices)
    {
        dicesNumberText.text = dices[0].GetCount().ToString() + "\n" + dices[1].GetCount().ToString() + "\n" + dices[2].GetCount().ToString()
           + "\n" + dices[3].GetCount().ToString() + "\n" + dices[4].GetCount().ToString();
    }

    public void UpdateTextCurrentScore(string text)
    {
        currentScoreText.text = text + " :C";
    }

    public void UpdateTextActiveScore(string text)
    {
        activeScoreText.text = text + " :A";
    }

    public void UpdateTextTotalScore(string text)
    {
        activeScoreText.text = text + " :T";
    }
}
