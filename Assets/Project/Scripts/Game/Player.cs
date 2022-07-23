using System;

public sealed class Player
{
    public string name;
    private int _countMinus;
    private bool _barrel;
    private int _score;
    
    public bool Barrel { get { return _barrel; } set { _barrel = value; } }
    public int CountMinus { get { return _countMinus; } set { _countMinus = value; } }
    public int Score
    {
        get { return _score; }
        set
        {
            if (ScoreChanged != null)
            {
                ValueChangingEventArgs vcea = new ValueChangingEventArgs(_score, value);
                ScoreChanged(this, vcea);
            }
            _score = value;
        }
    }

    public event EventHandler<ValueChangingEventArgs> ScoreChanged;

    public Player(string _name)
    {
        name = _name;
        _score = 0;
        _barrel = false;
        _countMinus = 0;
    }
}