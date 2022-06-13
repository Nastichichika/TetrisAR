using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{

    public static UIManager instance;
    public Text scoreText;
    public Text levelText;
    public Text layersText;
    [SerializeField]private GameObject gameOver;
    public TextMeshProUGUI highScoreText;


    private void Awake()
    {
        instance = this;
        gameOver.SetActive(false);
    }

    public void UpdateUI(int score, int level, int layers, int highScore)
    {
        scoreText.text = "Score: " + score;
        highScoreText.text = "" + highScore;
    }

    public void NewHighScore()
    {
        scoreText.text += "New high score";
    }
    public void gameIsOver() {
        gameOver.SetActive(true);
    }

}
