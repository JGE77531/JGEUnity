using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudmove : MonoBehaviour
{
    float Speed = 0.8f;
    private cloudManager cloudManager;
    private Vector2 startPos;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        cloudManager = GameObject.Find("ObjManager").GetComponent<cloudManager>();
        startPos = this.transform.position;
    }

    void Update()
    {
        if (!gameManager.isCounting)
        {
            transform.Translate(Vector2.left * Speed * Time.deltaTime);

            if (transform.position.x <= -11f)
            {
                cloudReset();
            }

            if (gameManager.state == GameState.Ready)
            {
                cloudReset();
            }
        }
    }

    public void cloudReset()
    {
        gameObject.SetActive(false); // 구름을 비활성화

        float posy = Random.Range(5f, 3f);
        Vector2 newPosition = transform.position;
        newPosition.y = posy;
        newPosition.x = 11;
        transform.position = newPosition; //랜덤 위치로 이동

        cloudManager.cloudObjectPool.Add(gameObject); //  다시 cloudObjectPool에 추가
    }
}