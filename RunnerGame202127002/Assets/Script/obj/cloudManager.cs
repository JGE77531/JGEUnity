using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cloudManager : MonoBehaviour
{
    public static GameManager gameManager;

    //cloud 복제
    float cloud_currentTime;
    float cloud_minTime = 3f;
    float cloud_maxTime = 8f;

    public float cloud_createTime = 1;
    public GameObject cloudFactory;

    public int cloudpoolSize = 8;
    public List<GameObject> cloudObjectPool;
    public Transform[] cloudspawnPoint;

    void Start()
    {
        cloud_createTime = Random.Range(cloud_minTime, cloud_maxTime);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //cloud 복제
        cloudObjectPool = new List<GameObject>();
        for (int i = 0; i < cloudpoolSize; i++)
        {
            GameObject cloud = Instantiate(cloudFactory);
            cloudObjectPool.Add(cloud);
            cloud.SetActive(false);
        }
    }

    public void cloudSpawner()
    {
        if (!gameManager.isCounting)
        {
            cloud_currentTime += Time.deltaTime; // 현재시간을 측정

            if (cloud_currentTime > cloud_createTime) // 현재 시간이 돌 생성 시간을 넘으면
            {
                if (cloudObjectPool.Count > 0)
                {
                    GameObject cloud = cloudObjectPool[0];
                    cloudObjectPool.Remove(cloud);
                    cloud.SetActive(true); // 돌을 활성화

                    if (cloudspawnPoint.Length > 0)
                    {
                        int index = Random.Range(0, cloudspawnPoint.Length);
                        cloud.transform.position = cloudspawnPoint[index].position;
                    }
                }

                cloud_currentTime = 0;
                cloud_createTime = Random.Range(cloud_minTime, cloud_maxTime);
            }
        }
    }
}
