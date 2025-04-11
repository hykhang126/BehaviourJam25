using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    protected State CurrentState;
    void Start()
    {
        CurrentState.Start();
    }

    // Update is called once per frame
    void Update()
    {
        CurrentState.Update();
    }

    public void SwitchState(State newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Start();
    }
}
