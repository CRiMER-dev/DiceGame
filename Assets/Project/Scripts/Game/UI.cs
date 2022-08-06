using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private GameObject readyButton;
    private GameObject throwButton;
    private GameObject saveScoreButton;
    private GameObject scoreTableView;
    private GameObject _tableContent;
    private RectTransform _contentRectTransform;

    public Object TextPrefab;

    private Text activePlayerText;
    private Text totalScoreText;
    private Text activeScoreText;
    private Text currentScoreText;
    private Text showHideText;

    private Game _game;

    private bool _showTableScore;

    private List<Text> _scoreText;

    private List<GameObject> _textGroups;

    private List<Text> _lastTexts;

    private void Awake()
    {
        //Buttons
        readyButton = GameObject.Find("ReadyButton");
        throwButton = GameObject.Find("ThrowButton");
        saveScoreButton = GameObject.Find("SaveScoreButton");
        scoreTableView = GameObject.Find("ScoreTableView");

        //Texts
        activePlayerText = GameObject.Find("ActivePlayer").GetComponent<Text>();
        totalScoreText = GameObject.Find("TotalScore").GetComponent<Text>();
        activeScoreText = GameObject.Find("ActiveScore").GetComponent<Text>();
        currentScoreText = GameObject.Find("CurrentScore").GetComponent<Text>();
        showHideText = GameObject.Find("ShowHideText").GetComponent<Text>();

        _game = GameObject.Find("Game").GetComponent<Game>();
        _tableContent = GameObject.Find("TableContent");
        
        _contentRectTransform = GameObject.Find("Content").GetComponent<RectTransform>();

        _scoreText = new List<Text>();

        _textGroups = new List<GameObject>();
        _lastTexts = new List<Text>();
    }

    private void Start()
    {
        activePlayerText.text = "Wait...";
        totalScoreText.text = "0 :T";
        activeScoreText.text = "0 :A";
        currentScoreText.text = "0 :C";

        _showTableScore = false;
        scoreTableView.SetActive(false);
        showHideText.text = "SHOW SCORE";
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
        activePlayerText.text = _game.Players[e.NewValue].Name;
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

        if (_game.Players[_game.CurrentPlayer].CountMinus == 0 && _lastTexts[_game.CurrentPlayer].text != e.NewValue.ToString())
        {
            GameObject rowText = Instantiate(TextPrefab, _textGroups[_game.CurrentPlayer].transform) as GameObject;
            rowText.GetComponent<Text>().text = e.NewValue.ToString();

            if (_game.Players[_game.CurrentPlayer].Barrel)
                rowText.GetComponent<Text>().fontStyle = FontStyle.Bold;

            _lastTexts[_game.CurrentPlayer] = rowText.GetComponent<Text>();

            IncreaseHeightContet();
        }
    }

    public void OnGameStateChanged(object sender, StateChangingEventArgs e)
    {
        switch (e.NewValue)
        {
            case Game.State.THROW:
                SetActiveReady(false);
                SetActiveThrow(true);
                SetActiveSave(false);
                break;
            case Game.State.THROWING:
                SetActiveThrow(false);
                SetActiveSave(false);
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
            case Game.State.SAVE:
                SetActiveThrow(false);
                SetActiveSave(true);
                break;
            case Game.State.SAMOSVAL:
                SetActiveThrow(false);
                SetActiveSave(true);
                break;
            case Game.State.OVERSCORE:
                SetActiveThrow(false);
                SetActiveSave(true);
                break;
            case Game.State.WIN:
                SetActiveThrow(false);
                SetActiveSave(false);
                SetActiveReady(false);
                break;
        }
    }

    public void OnShowHideClick()
    {
        if (_showTableScore)
        {
            scoreTableView.SetActive(false);
            showHideText.text = "SHOW SCORE";
        }
        else
        {
            scoreTableView.SetActive(true);
            showHideText.text = "HIDE SCORE";
        }

        _showTableScore = !_showTableScore;
    }

    public void OnPlayerCreated(string name)
    {
        GameObject textGroup = new GameObject("Group " + name);
        textGroup.transform.parent = _tableContent.transform;
        textGroup.transform.localPosition = new Vector3(0, 0, 0);
        textGroup.transform.localRotation = Quaternion.identity;
        textGroup.transform.localScale = new Vector3(1, 1, 1);

        VerticalLayoutGroup verticalLayout = textGroup.AddComponent<VerticalLayoutGroup>();
        verticalLayout.childControlHeight = true;
        verticalLayout.childControlWidth = true;
        verticalLayout.childForceExpandHeight = false;
        verticalLayout.childForceExpandWidth = true;
                

        _textGroups.Add(textGroup);

        GameObject rowText = Instantiate(TextPrefab, textGroup.transform) as GameObject;
        rowText.name = "TableContentText_" + name;
        rowText.GetComponent<Text>().text = name;

        GameObject zeroText = Instantiate(TextPrefab, textGroup.transform) as GameObject;
        zeroText.GetComponent<Text>().text = "0";

        _lastTexts.Add(zeroText.GetComponent<Text>());
    }

    public void OnCountMinusChanged(object sender, ValueChangingEventArgs e)
    {
        if (e.NewValue == 1)
        {
            GameObject rowText = Instantiate(TextPrefab, _textGroups[_game.CurrentPlayer].transform) as GameObject;
            rowText.GetComponent<Text>().text = "-";
            _lastTexts[_game.CurrentPlayer] = rowText.GetComponent<Text>();
        }
        else if (e.NewValue == 2)
        {
            _lastTexts[_game.CurrentPlayer].text = "+";
        }

        IncreaseHeightContet();
    }

    private void IncreaseHeightContet()
    {
        foreach (GameObject textGroup in _textGroups)
        {
            if (textGroup.GetComponent<RectTransform>().rect.height > _contentRectTransform.rect.height)
                _contentRectTransform.sizeDelta = new Vector2(0, textGroup.GetComponent<RectTransform>().rect.height);
        }
    }
}
