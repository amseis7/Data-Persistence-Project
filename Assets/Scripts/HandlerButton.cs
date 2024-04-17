using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandlerButton : MonoBehaviour
{
    public TMP_InputField entryName;
    public TextMeshProUGUI BestScore;

    public void Awake()
    {
        entryName = GameObject.Find("EntryName").GetComponent<TMP_InputField>();
        BestScore = GameObject.Find("BestScoreText").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        BestScore.text = $"Best Score: {GameManager.Instance.PlayerNameBest} : {GameManager.Instance.scoreBest}";
    }

    public void StartGame()
    {
        GameManager.Instance.CurrentPlayerName = entryName.text;
        GameManager.Instance.currentState = GameManager.GameState.inGame;
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
    public void SceneHighScore()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
}
