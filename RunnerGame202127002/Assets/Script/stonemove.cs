using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stonemove : MonoBehaviour
{
    float Speed = 3f;
    private StoneManager stoneManager;
    private Vector2 startPos;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        stoneManager = GameObject.Find("StoneManager").GetComponent<StoneManager>();
        startPos = this.transform.position;
    }

    void Update()
    {
        if (!gameManager.isCounting)
        {
            transform.Translate(Vector2.left * Speed * Time.deltaTime);

            if (transform.position.x <= -11f)
            {
                stoneReset();
            }

            if (gameManager.state == GameState.Ready)
            {
                stoneReset();
            }
        }
    }

    public void stoneReset()
    {
        gameObject.SetActive(false); // 돌을 비활성화
        this.transform.position = startPos; // 처음 위치로 이동
        stoneManager.stoneObjectPool.Add(gameObject); // 돌을 다시 stoneObjectPool에 추가
    }
}