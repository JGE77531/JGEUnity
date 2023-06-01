using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stone2move : MonoBehaviour
{
    public float speed = 4f; // �̵� �ӵ�
    private Vector2 startPos;
    private stone2Manager stoneManager;

    public float minY = -2.5f; // �̵� �ּ� y ��ǥ
    public float maxY = -1.0f; // �̵� �ִ� y ��ǥ

    private GameManager gameManager;

    private bool movingUp = true; // ���� �̵� ���� (�������� �̵� ������ ����)

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
            // �������� �̵�
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            // ���� ���� �����ϸ� �ʱ�ȭ
            if (transform.position.x <= -11f)
            {
                StoneReset();
            }

            // y ��ǥ �̵�
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

            // ���� �غ� �����̸� �ʱ�ȭ
            if (gameManager.state == GameState.Ready)
            {
                StoneReset();
            }
        }
    }

    private void StoneReset()
    {
        gameObject.SetActive(false); // ���� ��Ȱ��ȭ
        transform.position = startPos; // ó�� ��ġ�� �̵�
        stoneManager.stoneObjectPool.Add(gameObject); // ���� �ٽ� stoneObjectPool�� �߰�
        movingUp = true; // �̵� ���� �ʱ�ȭ
    }
}