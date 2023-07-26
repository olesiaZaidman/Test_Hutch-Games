
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const int PLAYER_LAYER = 1;
    public const int INVADERS_LAYER = 2;
    public const int BRICK_LAYER = 3;
    public const int PLAYER_BULLET_LAYER = 4;
    public const int INVADERS_BULLET_LAYER = 5;

    public const int ENEMIES_DAMAGE_AMOUNT = 1;
    public const int PLAYER_DAMAGE_AMOUNT = 1;
    public const int VICTORY_POINTS = 10;
    public const int MIN_START_SCORE = 0;
    public const int MAX_PLAYER_LIVES = 2;

    //public static int Score { get; private set; } =  MIN_START_SCORE;
    //public static int Lives { get; private set; } =  MAX_PLAYER_LIVES;

    public static bool GameOver { get; private set; } = false;
    public static bool WinGame { get; private set; } = false;

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
    //    ResetLivesAndScore();
    //}

    public static void ResetLivesAndScore()
    {
        Lives = MAX_PLAYER_LIVES;
        Score = MIN_START_SCORE;
    }


    void Update()
    {
        //if (!GameOver)
        //{
        //    Debugger();
        //}

        if (GameOver)
        {
            UIManager.Instance.UpdateUITextScore();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SetGameOver(false);
                RestartGame();
            }
        }

        if (WinGame)
        {
            UIManager.Instance.UpdateUITextScore();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                RestartGame();
                SetWinGameLevel(false);
            }
        }
    }

    private void RestartGame()
    {
        // Reload scene and reset currentScore & lives for a new game:
        ResetLivesAndScore();
        SceneLoader.Instance.ReloadCurrentScene();
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

    //void Debugger()
    //{
    //    if (Input.GetKeyDown(KeyCode.L))
    //    {
    //        SetGameOver(true);            
    //    }
    //    if (Input.GetKeyDown(KeyCode.P))
    //    {
    //        SetWinGameLevel(true);
    //    }
    //}


}
