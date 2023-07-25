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


    static int damageAmount = 1;
    public static int DamageAmount
    {
        get { return damageAmount; }
    }

    static int victoryPoints = 10;
    public static int VictoryPoints
    {
        get { return victoryPoints; }
    }


    static int lives; //=2
    static int maxPlayerLives = 2;

    static int currentScore; //=0
    static int minStartingScore = 0;

    static bool gameOver = false;
    static bool winGame = false;


    public static bool GameOver
    {
        get { return gameOver; }
        private set { gameOver = value; }
    }

    public static bool WinGame
    {
        get { return winGame; }
        private set { winGame = value; }
    }
    public static int Lives
    {
        get { return lives; }
        private set { lives = value; }
    }

    public static int MaxPlayerLives
    {
        get { return maxPlayerLives; }
       // private set { maxPlayerLives = value; }
    }
    public static int Score
    {              
      get { return currentScore; }
      private  set { currentScore = value; }
    }
    public static int MinStartingScore
    {
        get { return minStartingScore; }
      //  private set { minStartingScore = value; }
    }

    void Start()
    {
        SetGameValue(ref lives, MaxPlayerLives);
        SetGameValue(ref currentScore, MinStartingScore);
    }

    void Update()
    {
        if (GameOver)
        {               
            if (Input.GetKeyDown(KeyCode.Space))
            {

                // Reload scene and reset currentScore & lives for a new game:

                SetGameValue(ref lives, MaxPlayerLives);                 // GameManager.ResetLives();
                SetGameValue(ref currentScore, MinStartingScore);                // GameManager.ResetScore();
                SceneLoader.Instance.ReloadCurrentScene();

            }
        }

        if (WinGame)
        {
            SceneLoader.Instance.ReloadCurrentScene();
            // Reload scene for a new attack wave
        }
    }


    public static void ResetLives()
    {
        lives = maxPlayerLives;
    }

    public static void ResetScore()
    {
        currentScore = minStartingScore;
    }

    public static void SetLives(int i)
    {
        lives = i;
    }

    public static void SetScore(int i)
    {
        currentScore = i;
    }

    void SetGameValue(ref int value, int i)
    {
        value = i;
    }
    public static void AddPoints(int value, int points)
    {
        value += points;
    }

    public static void SubstractPoints(int value, int points)
    {
        value -= points;
    }

    public static void SetGameOver(bool isGameOver)
    {
        gameOver = isGameOver;
    }

    public static void SetWinGameLevel(bool isWin)
    {
        winGame = isWin;
    }

}
