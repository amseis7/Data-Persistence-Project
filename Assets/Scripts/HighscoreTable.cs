using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighscoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTranformList;

    private void Awake()
    {
        entryContainer = transform.Find("HighscoreEntryContainer");
        entryTemplate = entryContainer.Find("HighscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        var scores = GameManager.Instance.tableScore.OrderByDescending(s => s.scoreBest);
        
        highscoreEntryTranformList = new List<Transform>();
        foreach(var score in scores)
        {
            CreateHighscoreEntryTransform(score.PlayerNameBest, score.scoreBest, entryContainer, highscoreEntryTranformList);
        }
    }

    private void CreateHighscoreEntryTransform(string playerName, int score, Transform container, List<Transform> transformList)
    {
        float templateHeight = 35f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = $"{rank}\"TH\"";
                break;
            case 1:
                rankString = "1ST";
                break;
            case 2:
                rankString = "2ND";
                break;
            case 3:
                rankString = "3RD";
                break;
        }

        entryTransform.Find("PosText").GetComponent<TextMeshProUGUI>().text = rankString;

        entryTransform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = score.ToString();

        entryTransform.Find("NameText").GetComponent<TextMeshProUGUI>().text = playerName;

        entryTransform.Find("Background").gameObject.SetActive(rank % 2 == 1);
        transformList.Add(entryTransform);

        if (rank == 1)
        {
            entryTransform.Find("PosText").GetComponent<TextMeshProUGUI>().color = Color.green;

            entryTransform.Find("ScoreText").GetComponent<TextMeshProUGUI>().color = Color.green;

            entryTransform.Find("NameText").GetComponent<TextMeshProUGUI>().color = Color.green;
        }
    }

    public void BackMenu()
    {
        SceneManager.LoadScene(0);
    }
}
