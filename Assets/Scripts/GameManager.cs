using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using TetraCreations.Attributes;
using DG.Tweening;

public enum GameState
{
    InitGame,
    GameOn,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static event System.Action<Player> OnGameOver;
    public static event System.Action<GameState> OnGameStateChange;

    #region 
    [Title("Settings")]
    public TextMeshProUGUI textSwipe_1;
    public TextMeshProUGUI textSwipe_2;
    public TextMeshProUGUI textTouchCount_1;
    public TextMeshProUGUI textTouchCount_2;
    public TextMeshProUGUI textLineAngle_1;
    public TextMeshProUGUI textLineAngle_2;
    public TextMeshProUGUI textDefend_1;
    public TextMeshProUGUI textDefend_2;
    public TextMeshProUGUI textAllTouchCount;
    public int step;
    [ReadOnly] public int currentStep;
    public float stepDistance;
    public float delay;
    public Transform playerParent;

    public GameState currentState;
    #endregion

    [Title("Panel Reference")]
    public GameObject MenuPanel;
    public GameObject GameOverPanel;

    public void UpdateGameState(GameState newState)
    {
        currentState = newState;

        switch(currentState)
        {
            case GameState.InitGame:
                InitGame();
                break;
            case GameState.GameOn:
                GameOnHandler();
                break;
            case GameState.GameOver:
                GameOverHandler();
                break;
        }

        OnGameStateChange(currentState);

        MenuPanel.SetActive(currentState == GameState.InitGame);
        GameOverPanel.SetActive(currentState == GameState.GameOver);
    }

    void InitGame()
    {
        playerParent.position = Vector3.zero;
        currentStep = 0;
    }

    void GameOnHandler()
    {

    }

    void GameOverHandler()
    {
        if(currentStep >= 3)
        {
            OnGameOver(Player.Player_1);
        }
        if (currentStep <= -3)
        {
            OnGameOver(Player.Player_2);
        }
    }

    private void Awake()
    {
        Instance = this;
        TouchInputManager.OnSwipeGesture += SwipeHandler;
        PlayerController.OnAttackReceived += MovePlayerParent;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateGameState(GameState.InitGame);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void OnPlayButtonClick()
    {
        UpdateGameState(GameState.GameOn);
    }

    public void OnRestartButtonClick()
    {
        UpdateGameState(GameState.InitGame);
    }

    void MovePlayerParent(Player attackedPlayer)
    {
        if(attackedPlayer == Player.Player_1)
        {
            playerParent.DOMoveZ(playerParent.position.z - stepDistance, delay);
            currentStep++;
        }
        else if (attackedPlayer == Player.Player_2)
        {
            playerParent.DOMoveZ(playerParent.position.z + stepDistance, delay);
            currentStep--;
        }

        if(currentStep >= 3)
        {
            UpdateGameState(GameState.GameOver);
        }
        else if (currentStep <= -3)
        {
            UpdateGameState(GameState.GameOver);
        }
    }

    void SwipeHandler(Player player, Direction dir)
    {
        if(player == Player.Player_1)
        {
            textSwipe_1.text = dir.ToString();
        }
        if (player == Player.Player_2)
        {
            textSwipe_2.text = dir.ToString();
        }
    }
}
