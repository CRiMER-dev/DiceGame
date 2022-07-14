using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    private enum State{ NULL, READY, THROW, CALC, CHOISE };
    private State gameState;


    public List<Player> players;
    public int current_player = 0;

    private DiceScript[] dices = new DiceScript[5];
    private UI ui;

    private void Awake()
    {
        gameState = State.NULL;

        players = new List<Player>();
        current_player = 0;

        dices[0] = GameObject.Find("dice1").GetComponent<DiceScript>();
        dices[1] = GameObject.Find("dice2").GetComponent<DiceScript>();
        dices[2] = GameObject.Find("dice3").GetComponent<DiceScript>();
        dices[3] = GameObject.Find("dice4").GetComponent<DiceScript>();
        dices[4] = GameObject.Find("dice5").GetComponent<DiceScript>();

        ui = GameObject.Find("UI").GetComponent<UI>();
    }

    private void Update()
    {
        if (gameState == State.NULL)
        {
            ui.SetActiveReady(true);
        }
        if (gameState == State.READY)
        {
            ui.SetActiveThrow(true);
        }
        if (gameState == State.CALC)
        {
            int score = CalcScore();

            gameState = State.CHOISE;

            ui.UpdateTextCurrentScore(score.ToString());

            if (score == 0)
            {
                ui.SetActiveReady(true);

                if (!players[current_player].barrel || players[current_player].total_score < 0)
                {

                }
                else
                {
                    players[current_player].count_minus++;
                    if (players[current_player].count_minus >= 3)
                    {
                        players[current_player].count_minus = 0;
                        players[current_player].active_score = -100;

                        ui.UpdateTextActiveScore(players[current_player].active_score.ToString());
                    }
                }
            }
            else
            {


                ui.SetActiveSave(true);
                ui.SetActiveThrow(true);
            }
        }                    
    }

    public void Ready() {
        if (gameState == State.NULL)
        {
            players.Clear();
            players.Add(new Player("Player 1"));
            players.Add(new Player("Player 2"));
            current_player = 0;

            gameState = State.READY;

            ui.SetActiveReady(false);
            ui.UpdateTextActivePlayer(players[current_player].name.ToString());

            foreach (var dice in dices)
                dice.Reset();
        }
        if (gameState == State.CHOISE)
        {
            current_player++;
            if (current_player > 1)
                current_player = 0;
        }
    }

    public void Throw() {
        gameState = State.THROW;

        foreach(var dice in dices)
            dice.Throw();

        ui.SetActiveThrow(false);
        StartCoroutine("CheckDicesThrow");
    }

    IEnumerator CheckDicesThrow()
    {
        yield return new WaitForSeconds(1f);

        bool allSleep = true;
        foreach (var dice in dices)
        {
            if (!dice.Check())
                allSleep = false;
        }

        if (!allSleep)
        {
            StartCoroutine("CheckDicesThrow");
        }
        else
        {
            foreach (var dice in dices)
                dice.RegularDiceCount();

            ui.UpdateTextCount(dices);
            gameState = State.CALC;
        }
    }

    public void Save()
    {
        players[current_player].total_score += players[current_player].active_score;
        players[current_player].active_score = 0;
        players[current_player].count_minus = 0;

        current_player++;
        if (current_player > 1)
            current_player = 0;

        foreach (var dice in dices)
        {
            //dice.SetActive(false);
            //dice.SetActiveDice(true);
        }
        //throwButton.gameObject.SetActive(true);
        //saveScoreButton.gameObject.SetActive(false);

        //playerText.text = players[current_player].name;
        //totalScoreText.text = players[current_player].total_score.ToString();
        //activeScoreText.text = players[current_player].active_score.ToString();

    }



    public int CalcScore()
    {
        List<List<DiceScript>> test = new List<List<DiceScript>>();
        for (int i = 0; i < 6; i++)
            test.Add(new List<DiceScript>());

        foreach (var d in dices)
        {
            if (d.GetActive())
                test[d.GetCount() - 1].Add(d);
        }

        int score = 0;
        for (int i = 0; i < 6; i++)
        {
            if (test[i].Count == 5)
            {
                score += (i + 1 == 1) ? (i + 1) * 1000 : (i + 1) * 100;
                SetActiveDices(test[i], false);
            }
            else if (test[i].Count == 4)
            {
                score += (i + 1 == 1) ? (i + 1) * 200 : (i + 1) * 20;
                SetActiveDices(test[i], false);
            }
            else if (test[i].Count == 3)
            {
                score += (i + 1 == 1) ? (i + 1) * 100 : (i + 1) * 10;
                SetActiveDices(test[i], false);
            }
            else
            {
                if (i + 1 == 1)
                {
                    score += 10 * test[i].Count;
                    SetActiveDices(test[i], false);
                }
                else if (i + 1 == 5)
                {
                    score += 5 * test[i].Count;
                    SetActiveDices(test[i], false);
                }
            }
        }
        
        return score;
    }

    private void SetActiveDices(List<DiceScript> dd, bool value)
    {
        foreach (var d in dd)
        {
            d.SetActive(value);
        }
    }
}

public class Player
{
    public string name;
    public int total_score;
    public int active_score;
    public int count_minus;
    public bool barrel;

    public Player(string _name)
    {
        name = _name;
        total_score = 0;
        active_score = 0;
        barrel = false;
        count_minus = 0;
        Debug.Log("Create player " + _name);
    }
}