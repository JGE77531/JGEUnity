using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class igloomove : MonoBehaviour
{
    public float Speed = 1.5f;
    private iglooManager iglooManager;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        iglooManager = GameObject.Find("ObjManager").GetComponent<iglooManager>();

        Size_pos_set();
    }

    void Update()
    {
        if (!gameManager.isCounting)
        {
            transform.Translate(Vector2.left * Speed * Time.deltaTime);

            if (transform.position.x <= -11f)
            {
                iglooholeReset();
            }

            if (gameManager.state == GameState.Ready)
            {
                iglooholeReset();
            }
        }
    }

    public void iglooholeReset()
    {
        gameObject.SetActive(false); // 비활성화

        Size_pos_set();

        iglooManager.iglooObjectPool.Add(gameObject); // 다시 iglooholeObjectPool에 추가
    }

    void Size_pos_set()
    {
        float posy = Random.Range(-1.5f, 1.5f);
        Vector2 newPosition = transform.position;
        newPosition.y = posy;
        newPosition.x = 12;
        transform.position = newPosition; // 랜덤 위치로 이동

        Vector3 newScale = Vector3.one; // 초기 스케일을 (1, 1, 1)로 설정

        if (posy <= 1.5f && posy >= 0f)
        {
            newScale = new Vector3(4f, 4f, 1f); // 범위에 해당하는 경우 스케일을 (4, 4, 1)로 조정
            transform.Translate(Vector3.forward * 2f); // z 축을 뒤로 2만큼 이동
            Speed = 1.5f;
        }
        else
        {
            newScale = new Vector3(6f, 6f, 6f);  // 범위에 해당하지 않는 경우 스케일을 (1, 1, 1)로 설정
            transform.position = new Vector3(transform.position.x, transform.position.y, 1f); // z 축을 1으로 설정
            Speed = 2f;
        }

        transform.localScale = newScale;
    }
}