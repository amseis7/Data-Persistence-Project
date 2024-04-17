using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreNameText;
    public GameObject GameOverText;
    public GameObject PauseGameGUI;
    
    private bool m_Started = false;
    private string m_PlayerName;
    private int m_score;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance != null)
        {
            //GameManager.Instance.LoadBestScore();
            m_PlayerName = GameManager.Instance.PlayerNameBest;
            m_score = GameManager.Instance.scoreBest;
            BestScoreNameText.text = $"Best Score : {m_PlayerName} : {m_score}";
        }
        else
        {
            m_PlayerName= string.Empty;
        }

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.currentState == GameState.inGame)
        {
            PauseGame();
            
            
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.currentState == GameState.inPause)
        {
            ResumeGame();
            
            
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void ShowBestScore()
    {
        if (GameManager.Instance.IsBestScore(m_Points))
        {
            GameManager.Instance.PlayerNameBest = GameManager.Instance.CurrentPlayerName;
            GameManager.Instance.scoreBest = m_Points;
            BestScoreNameText.text = $"Best Score : {GameManager.Instance.CurrentPlayerName} : {m_Points}";
        }

        GameManager.Instance.HandleHighScore(GameManager.Instance.CurrentPlayerName, m_Points);
        GameManager.Instance.SaveBestScore();
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    public void PauseGame()
    {
        GameManager.Instance.currentState = GameState.inPause;
        Time.timeScale = 0;
        PauseGameGUI.SetActive(true);
    }

    public void ResumeGame()
    {
        GameManager.Instance.currentState = GameState.inGame;
        Time.timeScale = 1;
        PauseGameGUI.SetActive(false);
    }

    public void BackMenu()
    {
        SceneManager.LoadScene(0);
    }
}
