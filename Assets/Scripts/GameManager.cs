using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string CurrentPlayerName;
    public string PlayerNameBest;
    public int scoreBest;

    public GameState currentState;

    public enum GameState
    {
        mainMenu,
        inGame,
        inPause,
    }

    public List<SaveDataScore> tableScore = new List<SaveDataScore>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadBestScore();

    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = GameState.mainMenu;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    [System.Serializable]
    class HighScoresTable
    {
        public List<SaveDataScore> highScoreList = new List<SaveDataScore>();

        public HighScoresTable()
        {
            // Serializer
        }

        public HighScoresTable(List<SaveDataScore> n_highScoreList)
        {
            highScoreList = n_highScoreList;
        }
    }

    [System.Serializable]
    public class SaveDataScore
    {
        public string PlayerNameBest;
        public int scoreBest;

        public SaveDataScore(string n_PlayerNameBest, int n_ScoreBest)
        {
            PlayerNameBest = n_PlayerNameBest;
            scoreBest = n_ScoreBest;
        }
    }

    public void SaveBestScore()
    {
        HighScoresTable table = new HighScoresTable(tableScore);
        string json = JsonUtility.ToJson(table, true);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadBestScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            HighScoresTable fromJson = JsonUtility.FromJson<HighScoresTable>(json);

            foreach (SaveDataScore item in fromJson.highScoreList)
            {
                tableScore.Add(item);
            }

            var bestPlayer = tableScore.OrderByDescending(s => s.scoreBest).First();

            PlayerNameBest = bestPlayer.PlayerNameBest;
            scoreBest = bestPlayer.scoreBest;
        }
    }

    public void HandleHighScore(string n_PlayerName, int n_score)
    {
        if (tableScore.Count < 10)
        {
            SaveDataScore highScore = new SaveDataScore(n_PlayerName, n_score);
            tableScore.Add(highScore);
        }
        else
        {
            SaveDataScore lowertScore = tableScore.OrderBy(s => s.scoreBest).First();
            if (n_score > lowertScore.scoreBest)
            {
                SaveDataScore highScore = new SaveDataScore(n_PlayerName, n_score);
                tableScore.Remove(lowertScore);
                tableScore.Add(highScore);
            }
        }
    }

    public bool IsBestScore(int newScore)
    {
        if (newScore > scoreBest)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    
}