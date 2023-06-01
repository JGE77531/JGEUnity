using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iglooManager : MonoBehaviour
{
    public static GameManager gameManager;

    //igloo ����
    float igloo_currentTime;
    float igloo_minTime = 4f;
    float igloo_maxTime = 6f;

    public float igloo_createTime = 1;
    public GameObject iglooFactory;

    public int igloopoolSize = 5;
    public List<GameObject> iglooObjectPool;
    public Transform[] igloospawnPoint;

    void Start()
    {
        igloo_createTime = Random.Range(igloo_minTime, igloo_maxTime);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //igloo ����
        iglooObjectPool = new List<GameObject>();
        for (int i = 0; i < igloopoolSize; i++)
        {
            GameObject igloo = Instantiate(iglooFactory);
            iglooObjectPool.Add(igloo);
            igloo.SetActive(false);
        }
    }

    public void iglooSpawner()
    {
        if (!gameManager.isCounting)
        {
            igloo_currentTime += Time.deltaTime; // ����ð��� ����

            if (igloo_currentTime > igloo_createTime) // ���� �ð��� �� ���� �ð��� ������
            {
                if (iglooObjectPool.Count > 0)
                {
                    GameObject iglooObject = iglooObjectPool[0]; // �������� igloo���� iglooObject�� ����
                    iglooObjectPool.Remove(iglooObject);
                    iglooObject.SetActive(true); // ���� Ȱ��ȭ

                    if (igloospawnPoint.Length > 0)
                    {
                        int index = Random.Range(0, igloospawnPoint.Length);
                        iglooObject.transform.position = igloospawnPoint[index].position;
                    }
                }

                igloo_currentTime = 0;
                igloo_createTime = Random.Range(igloo_minTime, igloo_maxTime);
            }
        }
    }
}
