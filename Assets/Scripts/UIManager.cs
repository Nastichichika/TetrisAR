using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;
    public Text scoreText;
    public Text levelText;
    public Text layersText;

    public Text highScoreText;


    private void Awake()
    {
        instance = this;
    }

    public void UpdateUI(int score, int level, int layers, int highScore)
    {
        scoreText.text = "Score: " + score;
    }
     public void NewHighScore()
    {
        scoreText.text += "New high score";
    }

}
