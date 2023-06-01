using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq; // 추가된 부분

// 점수와 이름을 담는 클래스
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
    public TextMeshProUGUI scoreText; // 게임 플레이 중 점수를 띄움 텍스트
    public TextMeshProUGUI bestScoreText; // 게임 종료 후 나오는 Nowname의 최고 점수
    public TextMeshProUGUI currentScoreText; // 게임 종료 후 나오는 Nowname의 현재 점수

    public TMP_InputField nickname;

    public string nowName; // 게임 시작할 때 nickname에 저장된(입력받은) 현재 플레이어 이름, currentScore에 매칭되어 저장됨
    //public List<int> rankScore = new List<int>();
    //public List<string> rankName = new List<string>();

    public TextMeshProUGUI RankNames;
    public TextMeshProUGUI RankScores;

    private int[] RankscoreIndex = new int[10];
    private int playCount = 0; // 플레이 횟수를 저장하는 변수

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

        // 게임 시작 시 플레이어 이름 리스트 업데이트
        UpdatePlayerNameList();
    }

    void Update()
    {
        if (gameManager.state == GameState.Ready)
        {
            scoreText.text = "";
            //플레이타임 초기화
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

            // playerName에 해당하는 bestScore 데이터를 가져와서 출력
            int playerBestScore = PlayerPrefs.GetInt("BestScore_" + nowName, 0);
            bestScoreText.text = "[Best Score]\n" + playerBestScore;
        }
    }

    public void IncreasePlayCount()
    {
        playCount++; // 플레이 횟수 증가
        if (playCount >= 10)
        {
            playCount = 10;
        }
        PlayerPrefs.SetInt("Playcount", playCount); // 플레이 횟수를 PlayerPrefs에 저장
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
        PlayerPrefs.SetInt("Playcount", playCount); // 플레이 횟수를 PlayerPrefs에 저장
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

        int rankCount = Mathf.Min(rankList.Count, 10); // 최대 10개의 순위를 표시

        string rankText = "";
        string nameText = "";
        for (int i = 0; i < playCount; i++)
        {
            int rank = i + 1;
            nameText += rank + "위 " + rankList[i].Name + "\n";
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