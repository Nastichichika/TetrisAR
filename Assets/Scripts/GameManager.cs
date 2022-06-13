using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int score = 0;
    private int levels;
    private int layersCleared;

    private int highScore;

    private bool gameIsOver = false;

    private float fallSpeed;

    private void Awake()
    {
        
        instance = this;
    }

    void Start()
    {
        setScore(score);
        Debug.Log("highScore START"+  highScore);
        PlayerPrefs.GetInt("highScore", highScore);
    }

    public void setScore(int amount)
    {
        score += amount;
        if (highScore < score)
        {
            Debug.Log("highScore" + highScore);
            PlayerPrefs.SetInt("highScore", score);
            UIManager.instance.UpdateUI(score, levels, layersCleared, highScore);
            UIManager.instance.NewHighScore();

        }
        CalculateLevel();
        SetHighScore();
        UIManager.instance.UpdateUI(score,levels,layersCleared, highScore);
    }

    public void SetHighScore()
    {
        if (highScore < score) {
            highScore = score;
            PlayerPrefs.SetInt("highScore", highScore);
            PlayerPrefs.Save();
        }
        // Update UI
    }

    public float ReadFallSpeed()
    {
        return fallSpeed;
    }

    public void LayersCleared(int numberOfLayers)
    {
        // TODO create enum amount
        if(numberOfLayers == 1)
        {
            setScore(100);
        }

        else if (numberOfLayers == 2)
        {
            setScore(200);
        }

        else if (numberOfLayers == 3)
        {
            setScore(400);
        }

        else if (numberOfLayers == 4)
        {
            setScore(800);
        }

        layersCleared += numberOfLayers;
        // UIHandler.instance.UpdateUI(score, levels, layersCleared, highScore);
        // Update UI
    }

    void CalculateLevel()
    {
        if(score <= 3000) {
            levels = 1;
            fallSpeed = 3f;
        }

        else if(score > 300 && score <= 700) {
            levels = 2;
            fallSpeed = 2.7f;
        }
        else if (score > 700 && score <= 1500) {
            levels = 3;
            fallSpeed = 2.4f;
        }
        else if (score > 20000 && score <= 32000) {
            levels = 4;
            fallSpeed = 2.15f;
        }
        else if (score > 32000 && score <= 45000) {
            levels = 5;
            fallSpeed = 1.9f;
        }

    }

    public bool ReadGameIsOver()
    {
        return gameIsOver;
    }

    public void SetGameIsStart()
    {
        gameIsOver = false;
    }

    public void SetGameIsOver()
    {
        gameIsOver = true;
        UIManager.instance.gameIsOver();
    }
}
