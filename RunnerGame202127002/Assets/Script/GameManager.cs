using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

//���� ����
public enum GameState
{
    Ready, //Ÿ��Ʋ
    Play, //���� ���� ��
    End //���� ����
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

    //���º� Canvas
    public GameObject ReadyCanvas;
    public GameObject PlayCanvas;
    public GameObject OverCanvas;

    //���� �ӵ� ����
    public TextMeshProUGUI Stagetext;
    private int stageCount = 1;
    private float playTime = 0;
    private float gameSpeed = 1f;
    private float speedIncreaseInterval = 15f;
    private float speedIncreaseAmount = 0.25f;

    //�Ͻ�����
    private bool isPaused = false;
    public bool isCounting = false;
    public GameObject Paused_background; //���� 30�� ���� ȭ��

    void Start()
    {

        gameSpeed = 1f;

        isPaused = false;
        isCounting = false;

        if (gameManager == null)
        {
            gameManager = this;
        }

        //���� �����ϸ� �����ϴ� �Լ��� [�� ����], [����Ʈ ����]
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
            Stagetext.text = "�������� " + stageCount;
            GamePlay();
        }
        if (state == GameState.End)
        {
            GameOver();
        }
    }

    //���� Ÿ��Ʋ
    public void GameReady()
    {
        Time.timeScale = 0;
        Paused_background.SetActive(false);
        counttext.gameObject.SetActive(false);
        ReadyCanvas.SetActive(true);
        PlayCanvas.SetActive(false);
        OverCanvas.SetActive(false);
    }
    //<��ư �̺�Ʈ> ���� ���� ��ư Ŭ�� �� ���� ����
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
    //3>2>1 ī��Ʈ �ڷ�ƾ
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


    //���� ���� ��
    public void GamePlay()
    {
        ReadyCanvas.SetActive(false);
        PlayCanvas.SetActive(true);
        OverCanvas.SetActive(false);

        PlayCanvas.SetActive(true);
        state = GameState.Play;
        OnGamePlay.Invoke();

        //�÷��� �ð� ����
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
            Time.timeScale = 0f; // ���� �Ͻ�����
        }
        else
        {
            //�簳 ī��Ʈ�ٿ�
            StartCoroutine(Countdown());
            //Time.timeScale = gameSpeed;
        }
    }

    //���� ����
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