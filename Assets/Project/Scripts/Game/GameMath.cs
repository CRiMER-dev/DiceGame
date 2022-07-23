using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public sealed class GameMath : MonoBehaviour
{
    private DiceScript[] _dices = new DiceScript[5];
    private int _inactiveCount = 0;
    public int InactiveCount { get { return _inactiveCount; } set { _inactiveCount = value; } }

    public delegate void DicesEventHandler();
    public event DicesEventHandler ThrowingStopped;

    private Game _game;

    private void Awake()
    {
        _dices[0] = GameObject.Find("dice1").GetComponent<DiceScript>();
        _dices[1] = GameObject.Find("dice2").GetComponent<DiceScript>();
        _dices[2] = GameObject.Find("dice3").GetComponent<DiceScript>();
        _dices[3] = GameObject.Find("dice4").GetComponent<DiceScript>();
        _dices[4] = GameObject.Find("dice5").GetComponent<DiceScript>();

        _game = GameObject.Find("Game").GetComponent<Game>();
        ThrowingStopped += _game.OnThrowingStopped;
    }

    public int CalcScore()
    {
        List<List<DiceScript>> calcDices = new List<List<DiceScript>>();
        for (int i = 0; i < 6; i++)
            calcDices.Add(new List<DiceScript>());

        foreach (DiceScript dice in _dices)
        {
            if (dice.GetActive())
                calcDices[dice.GetCount() - 1].Add(dice);
        }

        int score = 0;
        for (int i = 0; i < 6; i++)
        {
            if (calcDices[i].Count == 5)
            {
                score += (i + 1 == 1) ? (i + 1) * 1000 : (i + 1) * 100;
                SetActiveDices(calcDices[i], false);
            }
            else if (calcDices[i].Count == 4)
            {
                score += (i + 1 == 1) ? (i + 1) * 200 : (i + 1) * 20;
                SetActiveDices(calcDices[i], false);
            }
            else if (calcDices[i].Count == 3)
            {
                score += (i + 1 == 1) ? (i + 1) * 100 : (i + 1) * 10;
                SetActiveDices(calcDices[i], false);
            }
            else
            {
                if (i + 1 == 1)
                {
                    score += 10 * calcDices[i].Count;
                    SetActiveDices(calcDices[i], false);
                }
                else if (i + 1 == 5)
                {
                    score += 5 * calcDices[i].Count;
                    SetActiveDices(calcDices[i], false);
                }
            }
        }

        return score;
    }

    private void SetActiveDices(List<DiceScript> dices, bool value)
    {
        if (!value)
            foreach (var dice in dices)
            {
                dice.SetActive(value);
                dice.SetInactivePos(_inactiveCount);
                _inactiveCount++;
            }
    }

    public void ResetDices()
    {
        InactiveCount = 0;
        foreach (DiceScript dice in _dices)
            dice.Reset();
    }

    public void ThrowDices()
    {
        foreach (DiceScript dice in _dices)
            dice.Throw();

        StartCoroutine("CheckDicesThrow");
    }

    IEnumerator CheckDicesThrow()
    {
        yield return new WaitForSeconds(.5f);

        bool allSleep = true;
        foreach (DiceScript dice in _dices)
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
            foreach (DiceScript dice in _dices)
                dice.RegularDiceCount();

            ThrowingStopped();
        }
    }
}
