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
        gameObject.SetActive(false); // ��Ȱ��ȭ

        Size_pos_set();

        iglooManager.iglooObjectPool.Add(gameObject); // �ٽ� iglooholeObjectPool�� �߰�
    }

    void Size_pos_set()
    {
        float posy = Random.Range(-1.5f, 1.5f);
        Vector2 newPosition = transform.position;
        newPosition.y = posy;
        newPosition.x = 12;
        transform.position = newPosition; // ���� ��ġ�� �̵�

        Vector3 newScale = Vector3.one; // �ʱ� �������� (1, 1, 1)�� ����

        if (posy <= 1.5f && posy >= 0f)
        {
            newScale = new Vector3(4f, 4f, 1f); // ������ �ش��ϴ� ��� �������� (4, 4, 1)�� ����
            transform.Translate(Vector3.forward * 2f); // z ���� �ڷ� 2��ŭ �̵�
            Speed = 1.5f;
        }
        else
        {
            newScale = new Vector3(6f, 6f, 6f);  // ������ �ش����� �ʴ� ��� �������� (1, 1, 1)�� ����
            transform.position = new Vector3(transform.position.x, transform.position.y, 1f); // z ���� 1���� ����
            Speed = 2f;
        }

        transform.localScale = newScale;
    }
}