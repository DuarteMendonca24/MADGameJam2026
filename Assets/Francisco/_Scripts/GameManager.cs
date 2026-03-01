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
    //private TransitionController transitionController;
    private Dictionary<int, KeyBoardManager> keyBoardManagers = new Dictionary<int, KeyBoardManager>();

    public GameObject player;

    public static GameManager Instance;

    public GameState State;

    public LettersOrderManager lettersOrderManager;

    public static event Action<GameState> OnGameStateChanged;

    private int currentGameLevel = 1;

    //Audio
    [SerializeField] private AudioClip BackgroundSound;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Movement playerMovement = player.GetComponent<Movement>();
            playerMovement.letterGoalReached += OnPlayerReachedLetter;
            playerMovement.playerDied += OnPlayerDeath;
            
            //transitionController = transition.GetComponent<TransitionController>();

            lettersOrderManager.Initialize();
            lettersOrderManager.wordCompleted += OnLevelCompleted;

            SetGameLevel(1);
            State = GameState.InGame;

            GetKeyboardManagers();
            PopulateGoalKey();

            StartLevel();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SoundManager.Instance.PlaySoundClip(BackgroundSound, transform, 1f);
    }

    private void GetKeyboardManagers()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            KeyBoardManager kbm = keyboardsParent.transform.GetChild(i).GetComponent<KeyBoardManager>();
            keyBoardManagers.Add(i + 1, kbm);
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
        PlayerDeathTransition();
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

    public void PlayerDeathTransition()
    {   
        // Por favor corrigir, obrigado
        //StartCoroutine(transitionController.PlayFullTransition());
        ResetLevel();
        PopulateGoalKey();
    }

    //Put everything back in place
    private void ResetLevel()
    {
        player.GetComponent<Movement>().Respawn(
            keyBoardManagers[currentGameLevel].playerSpawnPosition);
        
        keyBoardManagers[currentGameLevel].ResetKeyPositions();
        lettersOrderManager.ResetLetter(currentGameLevel);
    }

    public void OnLevelCompleted()
    {
        SetGameLevel(currentGameLevel + 1);
        TransitionIntoNextLevel();
    }

    private void TransitionIntoNextLevel()
    {
        //transition.SetActive(true);
        //transitionController.PlayTransitionOutAnimation();
        //yield return new WaitForSeconds(1f);
        //MOSTRAR BD???
        //bd.setActive(true)
        //transitionController.PlayTransitionInAnimation();
        //yield return new WaitForSeconds(10f);
        //bd.setActive(false)
        //transitionController.PlayTransitionOutAnimation();

        StartLevel();
        // transitionController.PlayTransitionInAnimation();
        // yield return new WaitForSeconds(1f);
        // transition.SetActive(false);

        lettersOrderManager.ShowRandomLetter(currentGameLevel);
        PopulateGoalKey();
    }

    private void StartLevel()
    {
        foreach (GameObject level in levels)
        {
            if (level.name.Contains(currentGameLevel.ToString()))
            {
                player.transform.position = keyBoardManagers[currentGameLevel].playerSpawnPosition;
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

