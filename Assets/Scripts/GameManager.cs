using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static int lives; //=2
    static int maxPlayerLives = 2;

    static int score = 0; //=0
    static bool gameOver = false;
    public static bool GameOver
    {
        get { return gameOver; }
        private set { gameOver = value; }
    }
    public static int Lives
    {
        get { return lives; }
        private set { lives = value; }
    }

    public static int MaxPlayerLives
    {
        get { return maxPlayerLives; }
        private set { maxPlayerLives = value; }
    }
    public static int Score
    {              
      get { return score; }
      private  set { score = value; }
    }


    void Start()
    {
        SetGameValue(ref lives, MaxPlayerLives);
        SetGameValue(ref score, 0);
    }


    public static void ResetLives()
    {
        lives = 2;
    }

    public static void ResetScore()
    {
        score = 0;
    }

    public static void SetLives(int i)
    {
        lives = i;
    }

    public static void SetScore(int i)
    {
        score = i;
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

}
