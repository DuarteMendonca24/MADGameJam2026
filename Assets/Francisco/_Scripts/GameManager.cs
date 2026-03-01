using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEditor.PackageManager.Requests;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    PauseMenuScreen,
    InGame,
    EndGame
}

public class GameManager : MonoBehaviour
{
    public GameObject[] levels;
    public GameObject transition;
    public GameObject keyboardsParent;
    private TransitionController transitionController;
    private Dictionary<int, KeyBoardManager> keyBoardManagers = new Dictionary<int, KeyBoardManager>();

    public GameObject player;

    public static GameManager Instance;

    public GameState State;

    public LettersOrderManager lettersOrderManager;

    public static event Action<GameState> OnGameStateChanged;

    private int currentGameLevel = 1;

 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            player.GetComponent<Movement>().letterGoalReached += OnPlayerReachedLetter;
            //transitionController = transition.GetComponent<TransitionController>();

            SetGameLevel(1);
            StartLevel();
            State = GameState.InGame;

            lettersOrderManager.Initialize();

            GetKeyboardManagers();
            PopulateGoalKey();
        }
        else
        {
            Destroy(gameObject);
        }

        

        
    }

    private void GetKeyboardManagers()
    {
        for(int i = 0; i < levels.Length; i++)
        {
            KeyBoardManager kbm = keyboardsParent.transform.GetChild(i).GetComponent<KeyBoardManager>();
            keyBoardManagers.Add(i+1,kbm);
        }
    }


    public int GetGameLevel() { return currentGameLevel; }

    public void SetGameLevel(int gameLevel) { this.currentGameLevel = gameLevel; }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.PauseMenuScreen:
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

    public void OnPlayerDeath()
    {
        StartCoroutine(PlayerDeathTransition());
    }

    private void PopulateGoalKey()
    {
        keyBoardManagers[currentGameLevel].SetGoalKey(lettersOrderManager.GetDisplayedLetter());
    }

    private void OnPlayerReachedLetter()
    {
        lettersOrderManager.ShowRandomLetter(currentGameLevel);
        PopulateGoalKey();
    }

    public IEnumerator PlayerDeathTransition()
    {
        transitionController.PlayTransitionOutAnimation();
        ResetLevel();
        yield return new WaitForSeconds(2f);
        transitionController.PlayTransitionInAnimation();
    }

    //Put everything back in place
    private void ResetLevel()
    {
        keyBoardManagers[currentGameLevel].ResetKeyPositions();
        player.transform.position = keyBoardManagers[currentGameLevel].playerSpawnPosition;
    }

    public void OnLevelCompleted()
    {
        //enquanto fôr um nível "normal"
        if (currentGameLevel < 4)
        {
            SetGameLevel(currentGameLevel + 1);
            StartCoroutine(TransitionIntoNextLevel());
        }
        else
        {
            // se fôr o último nível
        }
    }

    private IEnumerator TransitionIntoNextLevel()
    {
        transitionController.PlayTransitionOutAnimation();
        yield return new WaitForSeconds(1f);
        //MOSTRAR BD???
        //bd.setActive(true)
        //transitionController.PlayTransitionInAnimation();
        //yield return new WaitForSeconds(10f);
        //bd.setActive(false)
        //transitionController.PlayTransitionOutAnimation();

        StartLevel();
        yield return new WaitForSeconds(1f);
        transitionController.PlayTransitionInAnimation();
    }

    private void StartLevel()
    {
        foreach (GameObject level in levels)
        {
            if (level.name.Contains(currentGameLevel.ToString()))
            {
                level.SetActive(true);
            }
            else
            {
                level.SetActive(false);
            }
        }
    }

    public void StartGame()
    {
        UpdateGameState(GameState.InGame);
        //SceneManager.LoadScene("FinalScene");
    }

}

