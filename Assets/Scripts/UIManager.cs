
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    Canvas uiCanvas;
    Text uiScore;

    int fontSize = 30;

    string gameOverText = "\nGAME OVER!\nPRESS SPACE TO RESTART";
    string winGameText = "\nYOU WON!\nPRESS SPACE TO RESTART";
    public static UIManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        CreateUICanvas();
        CreateUIScoreText(fontSize);

    }
    void Start()
    {
        UpdateUIScoreText();
    }

    void CreateUICanvas()
    {
        uiCanvas = new GameObject("UI").AddComponent<Canvas>();
        uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }

    void CreateUIScoreText(int fontSize)
    {
        uiScore = uiCanvas.gameObject.AddComponent<Text>();
        uiScore.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        uiScore.fontSize = fontSize;
    }

    public void UpdateUITextScore()
    {
        UpdateUIScoreText();

        if (GameManager.GameOver)
        {
            AddAdditionalText(gameOverText);
        }

        if (GameManager.WinGame)
        {
            AddAdditionalText(winGameText);
        }
    }

    void UpdateUIScoreText()
    {
        uiScore.text = $"SCORE: {GameManager.Score:0000}";
    }

    void AddAdditionalText(string additionalText)
    {
        uiScore.text += additionalText;
    }

}
