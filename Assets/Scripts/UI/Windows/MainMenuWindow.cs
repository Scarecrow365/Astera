using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuWindow : MonoBehaviour, IWindow
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button deleteSave;

    public event Action OnStartButton;
    public event Action OnDeleteSaveButton;

    private void Awake()
    {
        startButton.onClick.AddListener(() => OnStartButton?.Invoke());
        deleteSave.onClick.AddListener(() => OnDeleteSaveButton?.Invoke());
    }

    public void ChangeState(State state)
    {
        gameObject.SetActive(state == State.MainMenu);
    }
}