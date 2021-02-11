using System;
using System.Collections;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public State CurrentState { get; private set; }

    public event Action<State> OnChangeState;

    public void StartGame()
    {
        StartCoroutine(ShowLevel());
    }

    public void ChangeState(State state)
    {
        CurrentState = state;
        OnChangeState?.Invoke(state);
    }

    public void GameOver()
    {
        ChangeState(State.GameOver);
    }

    private IEnumerator ShowLevel()
    {
        ChangeState(State.NextLevel);
        yield return new WaitForSeconds(3);
        ChangeState(State.Game);
    }
}