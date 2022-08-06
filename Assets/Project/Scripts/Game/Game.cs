using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Game : MonoBehaviour
{
    public enum State{ NULL, READY, THROW, THROWING, CALC, CHOISE, ZERO, SWITCH, SAVE, SAMOSVAL, OVERSCORE, WIN };
    
    private List<Player> _players;
    private int _currentPlayer;

    private UI _ui;
    private GameMath _gameMath;


    private int _currentScore;
    private int _activeScore;
    private State _gameState;

    // Properties
    public int CurrentScore { 
        get { return _currentScore; } 
        private set {
            if (CurrentScoreChanged != null)
            {
                ValueChangingEventArgs vcea = new ValueChangingEventArgs(_currentScore, value);
                CurrentScoreChanged(this, vcea);
            }
            _currentScore = value;
        } 
    }
    public int ActiveScore
    {
        get { return _activeScore; }
        private set
        {
            if (ActiveScoreChanged != null)
            {
                ValueChangingEventArgs vcea = new ValueChangingEventArgs(_activeScore, value);
                ActiveScoreChanged(this, vcea);
            }
            _activeScore = value;
        }
    }
    public int CurrentPlayer
    {
        get { return _currentPlayer; }
        private set
        {
            if (CurrentPlayerChanged != null)
            {
                ValueChangingEventArgs vcea = new ValueChangingEventArgs(_currentPlayer, value);
                CurrentPlayerChanged(this, vcea);
            }
            _currentPlayer = value;
        }
    }
    public State GameState
    {
        get { return _gameState; }
        private set
        {
            if (GameStateChanged != null)
            {
                StateChangingEventArgs vcea = new StateChangingEventArgs(_gameState, value);
                GameStateChanged(this, vcea);
            }
            _gameState = value;
        }
    }
    public List<Player> Players { get { return _players; } private set { _players = value; } }

    // Events
    public event EventHandler<ValueChangingEventArgs> ActiveScoreChanged;
    public event EventHandler<ValueChangingEventArgs> CurrentScoreChanged;
    public event EventHandler<ValueChangingEventArgs> CurrentPlayerChanged;

    public event EventHandler<StateChangingEventArgs> GameStateChanged;

    private void Awake()
    {
        _ui = GameObject.Find("UI").GetComponent<UI>();
        _gameMath = GameObject.Find("GameMath").GetComponent<GameMath>();

        CurrentScoreChanged += _ui.OnCurrentScoreChanged;
        ActiveScoreChanged += _ui.OnActiveScoreChanged;
        CurrentPlayerChanged += _ui.OnCurrentPlayerChanged;

        GameStateChanged += _ui.OnGameStateChanged;
    }

    private void Start()
    {
        _gameState = State.NULL;
        _ui.SetActiveReady(true);
        _ui.SetActiveSave(false);
        _ui.SetActiveThrow(false);
        _players = new List<Player>();
        _currentPlayer = 0;
    }

    public void OnThrowingStopped()
    {
        CurrentScore = _gameMath.CalcScore();

        Calc();
    }

    private void Calc()
    {
        if (CurrentScore == 0)
        {
            if (!Players[CurrentPlayer].Barrel && Players[CurrentPlayer].Score >= 0)
            {
                Players[CurrentPlayer].CountMinus++;
                if (Players[CurrentPlayer].CountMinus >= 3)
                {
                    Players[CurrentPlayer].CountMinus = 0;
                    CurrentScore = -100;
                }
                else
                {        
                    CurrentScore = 0;
                }
            }
            ActiveScore = 0;

            GameState = State.OVERSCORE;
        }
        else
        {
            if (Players[CurrentPlayer].Score + ActiveScore + CurrentScore == 555)
            {
                ActiveScore = 0;
                CurrentScore = 0;
                Players[CurrentPlayer].Score = 0;
                GameState = State.SAMOSVAL;
                return;
            }

            if (Players[CurrentPlayer].Score + ActiveScore + CurrentScore > 1000)
            {
                Players[CurrentPlayer].CountMinus++;
                if (Players[CurrentPlayer].CountMinus >= 3)
                {
                    Players[CurrentPlayer].CountMinus = 0;
                    CurrentScore = -100;
                }
                else
                {
                    CurrentScore = 0;
                }
                ActiveScore = 0;

                GameState = State.ZERO;
                return;
            }

            if (Players[CurrentPlayer].Score + ActiveScore + CurrentScore == 1000)
            {
                ActiveScore = 0;
                CurrentScore = 0;

                Players[CurrentPlayer].Score = 1000;
                GameState = State.WIN;
                return;
            }

            if (Players[CurrentPlayer].Barrel)
            {
                if (_gameMath.InactiveCount != 5)
                {
                    if (!Players[CurrentPlayer].BarrelCheck(Players[CurrentPlayer].Score + ActiveScore + CurrentScore))
                    {
                        GameState = State.CHOISE;
                    }
                    else
                    {
                        ActiveScore += CurrentScore;
                        CurrentScore = 0;
                        GameState = State.THROW;
                    } 
                }
                else
                    GameState = State.SAVE;
            }
            else
            {
                GameState = State.CHOISE;
            }
        }
    }

    public void Ready() {
        if (GameState == State.NULL)
        {
            Players.Clear();
            Players.Add(new Player("Player 1",_ui));
            Players.Add(new Player("Player 2",_ui));
            CurrentPlayer = 0;

            _gameMath.ResetDices();

            GameState = State.THROW;
        }
        if (GameState == State.SWITCH)
        {
            Players[CurrentPlayer].Score += ActiveScore;

            if (CurrentPlayer >= Players.Count - 1)
                CurrentPlayer = 0;
            else
                CurrentPlayer++;
            
            Players[CurrentPlayer].Score = Players[CurrentPlayer].Score;
            ActiveScore = 0;
            CurrentScore = 0;

            GameState = State.THROW;
        }
    }

    public void Throw() {
        if (GameState == State.THROW)
        {
            GameState = State.THROWING;

            _gameMath.ThrowDices();
        }
        else if (_gameState == State.CHOISE)
        {
            if (_gameMath.InactiveCount == 5) 
            {
                _gameMath.ResetDices();
            }
            ActiveScore += CurrentScore;
            CurrentScore = 0;

            GameState = State.THROWING;

            _gameMath.ThrowDices();
        }
    }

    public void Save()
    {
        if (GameState == State.SAMOSVAL)
        {
            Players[CurrentPlayer].CountMinus = 0;

            GameState = Game.State.SWITCH;
            _gameMath.ResetDices();
        }
        else if (GameState == State.OVERSCORE)
        {
            ActiveScore += CurrentScore;
            CurrentScore = 0;

            if (CurrentScore != 0)
                CheckDowngrade();

            GameState = Game.State.SWITCH;
            _gameMath.ResetDices();
        }
        else
        {
            if (GameState == State.CHOISE)
            {
                Players[CurrentPlayer].CountMinus = 0;
            }

            ActiveScore += CurrentScore;
            CurrentScore = 0;

            CheckDowngrade();

            GameState = Game.State.SWITCH;
            _gameMath.ResetDices();
        }
    }

    private void CheckDowngrade()
    {
        List<Player> downgradePlayers = new List<Player>();
        List<Player> comparePlayers = new List<Player>();

        foreach (Player player in Players)
        {
            if (player == Players[CurrentPlayer])
                continue;

            if ((player.Score > Players[CurrentPlayer].Score
                && player.Score < Players[CurrentPlayer].Score + ActiveScore)
                && !player.Barrel && player.Score >= 0)
            {
                downgradePlayers.Add(player);
            }
            else
            {
                comparePlayers.Add(player);
            }
        }

        foreach (Player player in downgradePlayers)
        {
            player.ScoreChanged -= _ui.OnScoreChanged;
            int score = Recursia(player, comparePlayers, -100);

            player.Score += score;
            player.CountMinus = 0;

            player.ScoreChanged += _ui.OnScoreChanged;
        }
    }

    private int Recursia(Player player, List<Player> comparePlayers, int oldScore)
    {
        int newScore = oldScore;
        foreach (Player comparePlayer in comparePlayers)
        {
            if (player.Score > comparePlayer.Score && player.Score + oldScore < comparePlayer.Score)
                newScore = Recursia(player, comparePlayers, oldScore);
        }
        return newScore;
    }
}