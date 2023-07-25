using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    Canvas uiCanvas;
    Text uiScore;

    public static UIManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        uiCanvas = new GameObject("UI").AddComponent<Canvas>();
        uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        uiScore = uiCanvas.gameObject.AddComponent<Text>();
        uiScore.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        uiScore.fontSize = 50;
    }


    void Update()
    {

    }

    public void DisplayScore(int _score)
    {
        if (uiScore != null)
        {
            uiScore.text = _score.ToString();
        }
        else Debug.Log("yourScoreText is null");
    }

    public void UpdateUITextScoreOnGameOver()
    {
        // uiScore.text = string.Format("SCORE: {0:0000}         {1}", GameManager.Score, GameManager.GameOver ? "GAME OVER!\n   PRESS SPACE TO RESTART" : "");

        uiScore.text = $"SCORE: {GameManager.Score:0000}         {(GameManager.GameOver ? "GAME OVER!\n   PRESS SPACE TO RESTART" : "")}";

    }

    void ShowGameOverMessage(string message)
    {

    }
}
