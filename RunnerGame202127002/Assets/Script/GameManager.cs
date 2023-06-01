using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

//게임 상태
public enum GameState
{
    Ready, //타이틀
    Play, //게임 시작 중
    End //게임 오버
}

public class GameManager : MonoBehaviour
{
    public GameState state;
    public static GameManager gameManager;

    public UnityEvent OnGamePlay;
    public UnityEvent OnGameReady;
    public UnityEvent OnGameOver;

    public TextMeshProUGUI overtext;
    public TextMeshProUGUI counttext;

    //상태별 Canvas
    public GameObject ReadyCanvas;
    public GameObject PlayCanvas;
    public GameObject OverCanvas;

    //게임 속도 증가
    public TextMeshProUGUI Stagetext;
    private int stageCount = 1;
    private float playTime = 0;
    private float gameSpeed = 1f;
    private float speedIncreaseInterval = 15f;
    private float speedIncreaseAmount = 0.25f;

    //일시정지
    private bool isPaused = false;
    public bool isCounting = false;
    public GameObject Paused_background; //투명도 30의 검은 화면

    void Start()
    {

        gameSpeed = 1f;

        isPaused = false;
        isCounting = false;

        if (gameManager == null)
        {
            gameManager = this;
        }

        //게임 시작하면 시작하는 함수들 [돌 생성], [포인트 생성]
        if (OnGamePlay == null)
            OnGamePlay = new UnityEvent();
        if (OnGameOver == null)
            OnGameOver = new UnityEvent();

        GameReady();
        state = GameState.Ready;
    }

    void Update()
    {
        if (state == GameState.Ready)
        {
            GameReady();
            isCounting = false;
        }
        if (state == GameState.Play)
        {
            Stagetext.text = "스테이지 " + stageCount;
            GamePlay();
        }
        if (state == GameState.End)
        {
            GameOver();
        }
    }

    //게임 타이틀
    public void GameReady()
    {
        Time.timeScale = 0;
        Paused_background.SetActive(false);
        counttext.gameObject.SetActive(false);
        ReadyCanvas.SetActive(true);
        PlayCanvas.SetActive(false);
        OverCanvas.SetActive(false);
    }
    //<버튼 이벤트> 게임 시작 버튼 클릭 시 게임 시작
    public void ReadyToPlay()
    {
        counttext.gameObject.SetActive(true);
        //Paused_background.SetActive(true);
        StartCoroutine(Countdown());
        if (isCounting)
        {
            state = GameState.Play;
            //Paused_background.SetActive(false);
        }
    }
    //3>2>1 카운트 코루틴
    private IEnumerator Countdown()
    {
        Paused_background.SetActive(true);
        counttext.gameObject.SetActive(true);
        Time.timeScale = 1;
        isCounting = true;
        for (int i = 3; i > 0; i--)
        {
            counttext.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        Paused_background.SetActive(false);
        counttext.gameObject.SetActive(false);
        Time.timeScale = gameSpeed;
        isCounting = false;
    }


    //게임 진행 중
    public void GamePlay()
    {
        ReadyCanvas.SetActive(false);
        PlayCanvas.SetActive(true);
        OverCanvas.SetActive(false);

        PlayCanvas.SetActive(true);
        state = GameState.Play;
        OnGamePlay.Invoke();

        //플레이 시간 측정
        playTime += Time.deltaTime;

        if (playTime >= speedIncreaseInterval)
        {
            IncreaseGameSpeed();
            playTime = 0f;
        }
    }
    private void IncreaseGameSpeed()
    {
        gameSpeed += speedIncreaseAmount;
        Time.timeScale = gameSpeed;
        stageCount += 1;
    }
    public void PauseGame()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Paused_background.SetActive(true);
            Time.timeScale = 0f; // 게임 일시정지
        }
        else
        {
            //재개 카운트다운
            StartCoroutine(Countdown());
            //Time.timeScale = gameSpeed;
        }
    }

    //게임 오버
    public void GameOver()
    {
        OnGameOver.Invoke();
        ReadyCanvas.SetActive(false);
        PlayCanvas.SetActive(false);
        OverCanvas.SetActive(true);

        Time.timeScale = 0;
        overtext.enabled = true;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        gameSpeed = 1;
        playTime = 0;
        stageCount = 1;
        GameReady();
        OverCanvas.SetActive(false);

        state = GameState.Ready;
    }
}