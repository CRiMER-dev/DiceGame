using System;

public sealed class Player
{
    private string _name;
    private int _countMinus;
    private bool _barrel;
    private int _score;
    
    public string Name { get { return _name; } set { _name = value; } }
    public bool Barrel { get { return _barrel; } set { _barrel = value; } }
    public int CountMinus
    {
        get { return _countMinus; }
        set
        {
            if (ScoreChanged != null)
            {
                ValueChangingEventArgs vcea = new ValueChangingEventArgs(_countMinus, value);
                CountMinusChanged(this, vcea);
            }
            _countMinus = value;
        }
    }
    public int Score
    {
        get { return _score; }
        set
        {
            _barrel = BarrelCheck(value);
            if (ScoreChanged != null)
            {
                ValueChangingEventArgs vcea = new ValueChangingEventArgs(_score, value);
                ScoreChanged(this, vcea);
            }
            _score = value;
        }
    }

    public event EventHandler<ValueChangingEventArgs> ScoreChanged;
    public event EventHandler<ValueChangingEventArgs> CountMinusChanged;
    
    public delegate void PlayerEventHandler(string name);
    public event PlayerEventHandler CreatePlayer;

    public Player(string name, UI ui = null)
    {
        _name = name;
        _score = 0;
        _barrel = false;
        _countMinus = 0;

        if (ui != null)
        {
            CreatePlayer += ui.OnPlayerCreated;
            ScoreChanged += ui.OnScoreChanged;
            CountMinusChanged += ui.OnCountMinusChanged;

            CreatePlayer(name);
        }
    }

    public bool BarrelCheck(int score)
    {
        return (score >= 200 && score <= 300) || (score >= 600 && score <= 700);
    }
}