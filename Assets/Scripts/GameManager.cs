using System;
using System.Collections.Generic; 
using System.Linq; 
using UniRx; 
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    public enum GameState
    {
        Menu,
        Game
    }

    [System.Serializable]
    public class Panel
    {
        [HideInInspector]
        public string name;
        public GameState state;
        public GameObject[] gameObjects;
    }

    public GameState state;
    public Panel[] panels;

    [Header("Game Parameters")]
    int score;
    public int TotalScore
    {
        get
        {
            int total = score;
            return total;
        }
    }
    [Header("Buttons State")]
    private bool _initialized;

    [SerializeField]
    Button startGameBttn;

    public delegate void OnCreateCircles();
    public static event OnCreateCircles CreateCircles;

    private void Start()
    {
        Initialized();
    }

    private void Initialized()
    {
        if (_initialized)
        {
            return;
        }

        score = PlayerPrefs.HasKey("score") ? PlayerPrefs.GetInt("score") : 0;
        UIController.Instance.UpdateScores();

        SetGameState(GameState.Menu);
        startGameBttn.onClick.AddListener(ButtonStartGame);

        _initialized = true;
    }

    private void OnValidate()
    {
        if (panels != null)
        {
            foreach (var panel in panels)
            {
                panel.name = panel.state.ToString();
            }
        }
    }

    public void SetGameState(GameState _state)
    {
        state = _state;

        foreach (var panel in panels)
        {
            if (panel.state != state)
            {
                foreach (var go in panel.gameObjects)
                {
                    if (go != null)
                    {
                        go.SetActive(false);
                    }
                }
            }
        }
        foreach (var panel in panels)
        {
            if (panel.state == state)
            {
                foreach (var go in panel.gameObjects)
                {
                    if (go != null)
                    {
                        go.SetActive(true);
                    }
                }
            }
        }
    }

    public void AddScore(int add)
    {
        score += add;

        PlayerPrefs.SetInt("score", score);
        UIController.Instance.UpdateScores();
    }

    private void ButtonMainMenu()
    {
        SetGameState(GameState.Menu);
    }

    private void ButtonStartGame()
    {
        SetGameState(GameState.Game);
        CreateCircles();
    }
}