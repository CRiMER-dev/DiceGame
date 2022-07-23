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

    private Game _game;

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

        _game = GameObject.Find("Game").GetComponent<Game>();
    }

    private void Start()
    {
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
        _game.Ready();
    }

    public void Throw()
    {
        _game.Throw();
    }

    public void Save()
    {
        _game.Save();
    }

    public void OnCurrentPlayerChanged(object sender, ValueChangingEventArgs e)
    {
        activePlayerText.text = _game.Players[e.NewValue].name;
    }

    public void OnCurrentScoreChanged(object sender, ValueChangingEventArgs e)
    {
        currentScoreText.text = e.NewValue.ToString() + " :C";
    }

    public void OnActiveScoreChanged(object sender, ValueChangingEventArgs e)
    {
        activeScoreText.text = e.NewValue.ToString() + " :A";
    }

    public void OnScoreChanged(object sender, ValueChangingEventArgs e)
    {
        totalScoreText.text = e.NewValue.ToString() + " :T";
    }

    public void OnGameStateChanged(object sender, StateChangingEventArgs e)
    {
        switch (e.NewValue)
        {
            case Game.State.THROW:
                SetActiveReady(false);
                SetActiveThrow(true);
                break;
            case Game.State.THROWING:
                SetActiveThrow(false);
                break;
            case Game.State.CHOISE:
                SetActiveSave(true);
                SetActiveThrow(true);
                break;
            case Game.State.ZERO:
                SetActiveSave(true);
                SetActiveThrow(false);
                break;
            case Game.State.SWITCH:
                SetActiveReady(true);
                SetActiveThrow(false);
                SetActiveSave(false);
                break;
        }
    }
}
