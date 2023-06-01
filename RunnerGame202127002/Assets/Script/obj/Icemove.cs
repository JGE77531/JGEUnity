using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icemove : MonoBehaviour
{
    float Speed = 3f;
    private IceManager IceManager;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        IceManager = GameObject.Find("ObjManager").GetComponent<IceManager>();
    }

    void Update()
    {
        if (!gameManager.isCounting)
        {
            transform.Translate(Vector2.left * Speed * Time.deltaTime);

            if (transform.position.x <= -11f)
            {
                IceholeReset();
            }

            if (gameManager.state == GameState.Ready)
            {
                IceholeReset();
            }
        }
    }

    public void IceholeReset()
    {
        gameObject.SetActive(false); // ���� ��Ȱ��ȭ

        float posy = Random.Range(-4f, -5f);
        Vector2 newPosition = transform.position;
        newPosition.y = posy;
        newPosition.x = 11;
        transform.position = newPosition; //���� ��ġ�� �̵�

        IceManager.IceObjectPool.Add(gameObject); // ���� �ٽ� IceholeObjectPool�� �߰�
    }
}