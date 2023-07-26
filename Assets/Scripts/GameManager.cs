using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    static public int playerLayer = 7; //GameManager.playerLayer
    static public int invadersLayer = 8; //GameManager.invadersLayer
    static public int brickLayer = 9;
    static public int playerBulletLayer = 10;
    static public int invadersBulletLayer = 11;

    public static bool GameOver { get; private set; } = false;
    public static bool WinGame { get; private set; } = false;

    public const int ENEMIES_DAMAGE_AMOUNT = 1;
    public const int PLAYER_DAMAGE_AMOUNT = 1;
    public const int VICTORY_POINTS = 10;
    public const int MIN_START_SCORE  = 0;
    public const int MAX_PLAYER_LIVES  = 2;

    //public static int Score { get; private set; } =  MIN_START_SCORE;
    //public static int Lives { get; private set; } =  MAX_PLAYER_LIVES;


    static int lives; //=2

    static int currentScore; //=0

    public static int Lives
    {
        get { return lives; }
        private set { lives = value; }
    }

    public static int Score
    {
        get { return currentScore; }
        private set { currentScore = value; }
    }

    public GameManager()
    {
        ResetLivesAndScore();
    }

    //void Start()
    //{
    //  //  ResetLivesAndScore();
    //}

    public static void ResetLivesAndScore()
    {
        Lives = MAX_PLAYER_LIVES;
        Score = MIN_START_SCORE;
    }


    void Update()
    {
        if (GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RestartGame();
            }
        }

        if (WinGame)
        {
            SceneLoader.Instance.ReloadCurrentScene();
            // Reload scene for a new attack wave
        }
    }

    private void RestartGame()
    {
        // Reload scene and reset currentScore & lives for a new game:
        ResetLivesAndScore();
        SceneLoader.Instance.ReloadCurrentScene();
    }

  
    public static void SetLives(int value)
    {
        Lives = value;
    }

    public static void SetScore(int value)
    {
        Score = value;
    }

    public static void IncreaseScore(int points)
    {
        Score += points;
    }

    public static void SubtractLives(int points)
    { 
        if (Lives > 0)
        {
            Lives -= points;
        }
        else
        { 
            Lives = 0; 
        }       
    }


    public static void SetGameOver(bool isGameOver)
    {
        GameOver = isGameOver;
    }

    public static void SetWinGameLevel(bool isWin)
    {
        WinGame = isWin;
    }


    //void ResetLivesAndScore()
    //{
    //    SetGameValue(ref lives, MaxPlayerLives);
    //    SetGameValue(ref currentScore, MinStartingScore);
    //}


    //void SetGameValue(ref int value, int i)
    //{
    //    value = i;
    //}
    //public static void AddPoints(int value, int points)
    //{
    //    value += points;
    //}

    //public static void SubstractPoints(int value, int points)
    //{
    //    value -= points;
    //}
}
