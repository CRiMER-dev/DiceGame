using System;

public class ValueChangingEventArgs : EventArgs
{
    public int OldValue { get; private set; }
    public int NewValue { get; private set; }

    public ValueChangingEventArgs(int OldValue, int NewValue)
    {
        this.OldValue = OldValue;
        this.NewValue = NewValue;
    }
}

public class StateChangingEventArgs : EventArgs
{
    public Game.State OldValue { get; private set; }
    public Game.State NewValue { get; private set; }

    public StateChangingEventArgs(Game.State OldValue, Game.State NewValue)
    {
        this.OldValue = OldValue;
        this.NewValue = NewValue;
    }
}