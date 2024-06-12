using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currState { get; private set; }

    public void Initialize(PlayerState _startState)
    {
        currState = _startState;
        currState.Enter();
    }

    public void ChangeState(PlayerState _newState)
    {
        Debug.Log($"Changing state from {currState.GetType().Name} to {_newState.GetType().Name}");
        currState.Exit();
        currState = _newState;
        currState.Enter();
    }

}