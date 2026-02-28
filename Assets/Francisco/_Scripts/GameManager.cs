using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;

    private int gameLevel = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        State = GameState.MainMenuScreen;
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.MainMenuScreen:
                Cursor.visible = true;
                break;
            case GameState.InGame:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case GameState.EndGame:
                break;
            default:
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }

    public int GetgameLevel() { return gameLevel; }

    public  void SetGameLevel(int gameLevel) { this.gameLevel = gameLevel; }

    public void StartGame()
    {
        UpdateGameState(GameState.InGame);
        //SceneManager.LoadScene("FinalScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}

public enum GameState
{
    MainMenuScreen,
    InGame,
    EndGame
}
