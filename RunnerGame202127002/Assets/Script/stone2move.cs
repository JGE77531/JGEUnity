using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stone2move : MonoBehaviour
{
    public float speed = 4f; // 이동 속도
    private Vector2 startPos;
    private stone2Manager stoneManager;

    public float minY = -2.5f; // 이동 최소 y 좌표
    public float maxY = -1.0f; // 이동 최대 y 좌표

    private GameManager gameManager;

    private bool movingUp = true; // 현재 이동 방향 (위쪽으로 이동 중인지 여부)

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        stoneManager = GameObject.Find("StoneManager").GetComponent<stone2Manager>();
        startPos = transform.position;
    }

    private void Update()
    {
        if (!gameManager.isCounting)
        {
            // 왼쪽으로 이동
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            // 왼쪽 끝에 도달하면 초기화
            if (transform.position.x <= -11f)
            {
                StoneReset();
            }

            // y 좌표 이동
            if (movingUp)
            {
                transform.Translate(Vector2.up * 6f * Time.deltaTime);
                if (transform.position.y >= maxY)
                    movingUp = false;
            }
            else
            {
                transform.Translate(Vector2.down * 6f*1.2f * Time.deltaTime);
                if (transform.position.y <= minY)
                    movingUp = true;
            }

            // 게임 준비 상태이면 초기화
            if (gameManager.state == GameState.Ready)
            {
                StoneReset();
            }
        }
    }

    private void StoneReset()
    {
        gameObject.SetActive(false); // 돌을 비활성화
        transform.position = startPos; // 처음 위치로 이동
        stoneManager.stoneObjectPool.Add(gameObject); // 돌을 다시 stoneObjectPool에 추가
        movingUp = true; // 이동 방향 초기화
    }
}