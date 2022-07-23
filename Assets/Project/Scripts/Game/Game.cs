using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Game : MonoBehaviour
{
    public enum State{ NULL, READY, THROW, THROWING, CALC, CHOISE, ZERO, SWITCH };
    
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
            if (ActiveScoreChanged != null)
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
            if (Players[CurrentPlayer].Barrel || Players[CurrentPlayer].Score < 0)
            {

            }
            else
            {
                Players[CurrentPlayer].CountMinus++;
                if (Players[CurrentPlayer].CountMinus >= 3)
                {
                    Players[CurrentPlayer].CountMinus = 0;
                    ActiveScore = 0;
                    CurrentScore = -100;
                }
                else
                {
                    ActiveScore = 0;
                    CurrentScore = 0;
                }
            }

            GameState = State.ZERO;
        }
        else
        {
            GameState = State.CHOISE;
        }
    }

    public void Ready() {
        if (GameState == State.NULL)
        {
            Players.Clear();
            Players.Add(new Player("Player 1"));
            Players.Add(new Player("Player 2"));
            CurrentPlayer = 0;

            Players[0].ScoreChanged += _ui.OnScoreChanged;
            Players[1].ScoreChanged += _ui.OnScoreChanged;

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

            _gameState = State.THROWING;

            _gameMath.ThrowDices();
        }
    }

    public void Save()
    {
        if (GameState == State.ZERO)
        {
            ActiveScore += CurrentScore;
            CurrentScore = 0;
        }
        else if (GameState == State.CHOISE)
        {
            ActiveScore += CurrentScore;
            CurrentScore = 0;

            Players[CurrentPlayer].CountMinus = 0;

            //CheckAdvance();
        }

        GameState = Game.State.SWITCH;
        _gameMath.ResetDices();

        //_players[_currentPlayer].Score += _activeScore + _currentScore;
        //_activeScore = 0;
        //_currentScore = 0;

        //_ui.SetActiveSave(false);
        //_ui.SetActiveThrow(false);
        //_ui.SetActiveReady(true);

        //_gameState = State.SWITCH;

        //_currentPlayer++;
        //if (_currentPlayer > 1)
        //    _currentPlayer = 0;

        //inactive_count = 0;
    }

    private void CheckAdvance()
    {
        foreach (Player player in Players)
        {
            if (player == Players[CurrentPlayer])
                continue;

            if (player.Score > Players[CurrentPlayer].Score
                && player.Score < Players[CurrentPlayer].Score + ActiveScore)
            {
                player.ScoreChanged -= _ui.OnScoreChanged;

                int score = -100;
                foreach (Player otherPlayer in Players)
                {
                    if (otherPlayer == player || otherPlayer == Players[CurrentPlayer])
                        continue;


                    if (player.Score > otherPlayer.Score && player.Score - 100 < otherPlayer.Score)
                        score -= 100;
                }
                
                player.Score += score;
                player.CountMinus = 0;

                player.ScoreChanged += _ui.OnScoreChanged;
            }
        }
    }
}