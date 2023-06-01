using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq; // �߰��� �κ�

// ������ �̸��� ��� Ŭ����
public class ScoreEntry
{
    public string Name { get; private set; }
    public int Score { get; private set; }

    public ScoreEntry(string name, int score)
    {
        Name = name;
        Score = score;
    }
}

public class DataManager : MonoBehaviour
{
    private GameManager gameManager;
    public int currentScore;
    private int bestScore;
    public TextMeshProUGUI scoreText; // ���� �÷��� �� ������ ��� �ؽ�Ʈ
    public TextMeshProUGUI bestScoreText; // ���� ���� �� ������ Nowname�� �ְ� ����
    public TextMeshProUGUI currentScoreText; // ���� ���� �� ������ Nowname�� ���� ����

    public TMP_InputField nickname;

    public string nowName; // ���� ������ �� nickname�� �����(�Է¹���) ���� �÷��̾� �̸�, currentScore�� ��Ī�Ǿ� �����
    //public List<int> rankScore = new List<int>();
    //public List<string> rankName = new List<string>();

    public TextMeshProUGUI RankNames;
    public TextMeshProUGUI RankScores;

    private int[] RankscoreIndex = new int[10];
    private int playCount = 0; // �÷��� Ƚ���� �����ϴ� ����

    public static DataManager dataManager = null;

    private float playTime;

    void Start()
    {
        playCount = PlayerPrefs.GetInt("Playcount", 0);

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (dataManager == null)
        {
            dataManager = this;
        }

        // ���� ���� �� �÷��̾� �̸� ����Ʈ ������Ʈ
        UpdatePlayerNameList();
    }

    void Update()
    {
        if (gameManager.state == GameState.Ready)
        {
            scoreText.text = "";
            //�÷���Ÿ�� �ʱ�ȭ
            playTime = 0;

            playCount = PlayerPrefs.GetInt("Playcount", 0);

            UpdatePlayerNameList();
            currentScore = 0;
            nowName = nickname.text;
            bestScore = PlayerPrefs.GetInt("BestScore_" + nowName, 0);
            PlayerPrefs.SetString("PlayerName", nowName);
        }   

        if (gameManager.state == GameState.Play && !gameManager.isCounting)
        {
            playTime += Time.deltaTime;
            currentScore = Mathf.FloorToInt(playTime);
            scoreText.text = "Score: " + currentScore;

        }

        if (gameManager.state == GameState.End)
        {
            playCount = PlayerPrefs.GetInt("Playcount", 0);
            ShowScoreRank();

            if (currentScore > bestScore)
            {
                IncreasePlayCount();
                bestScore = currentScore;
                currentScoreText.text = "Score : " + currentScore;
                bestScoreText.text = "[Best Score]\n" + bestScore;
                PlayerPrefs.SetInt("BestScore_" + nowName, bestScore);
            }

            currentScoreText.text = "Score : " + currentScore;

            // playerName�� �ش��ϴ� bestScore �����͸� �����ͼ� ���
            int playerBestScore = PlayerPrefs.GetInt("BestScore_" + nowName, 0);
            bestScoreText.text = "[Best Score]\n" + playerBestScore;
        }
    }

    public void IncreasePlayCount()
    {
        playCount++; // �÷��� Ƚ�� ����
        if (playCount >= 10)
        {
            playCount = 10;
        }
        PlayerPrefs.SetInt("Playcount", playCount); // �÷��� Ƚ���� PlayerPrefs�� ����
    }

    public int ScoreRank
    {
        get { return currentScore; }
        set { currentScore = value; }
    }

    public void RankReset()
    {
        List<ScoreEntry> rankList = new List<ScoreEntry>();
        rankList.Clear();
        PlayerPrefs.DeleteAll();
        //ShowScoreRank();

        playCount = 0;
        PlayerPrefs.SetInt("Playcount", playCount); // �÷��� Ƚ���� PlayerPrefs�� ����
    }

    public void ShowScoreRank()
    {
        List<ScoreEntry> rankList = new List<ScoreEntry>();

        foreach (string key in PlayerPrefs.GetString("PlayerNames", "").Split(','))
        {
            int score = PlayerPrefs.GetInt("BestScore_" + key, 0);
            rankList.Add(new ScoreEntry(key, score));
        }

        rankList.Sort((a, b) => b.Score.CompareTo(a.Score));

        int rankCount = Mathf.Min(rankList.Count, 10); // �ִ� 10���� ������ ǥ��

        string rankText = "";
        string nameText = "";
        for (int i = 0; i < playCount; i++)
        {
            int rank = i + 1;
            nameText += rank + "�� " + rankList[i].Name + "\n";
            rankText += rankList[i].Score + "\n";
        }

        RankNames.text = nameText;
        RankScores.text = rankText;
    }

    private void UpdatePlayerNameList()
    {
        string[] playerNames = PlayerPrefs.GetString("PlayerNames", "").Split(',');
        List<string> uniqueNames = new List<string>(playerNames);

        if (!uniqueNames.Contains(nowName) && !string.IsNullOrEmpty(nowName))
        {
            uniqueNames.Add(nowName);
            PlayerPrefs.SetString("PlayerNames", string.Join(",", uniqueNames.ToArray()));
        }
    }
}