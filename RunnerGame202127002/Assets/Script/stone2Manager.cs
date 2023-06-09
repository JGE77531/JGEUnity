using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stone2Manager : MonoBehaviour
{
    public static GameManager gameManager;
    private AABB aabb; // AABB 컴포넌트에 접근하기 위한 변수

    //복제
    float stone_currentTime;

    public float stone_createTime;

    float stone_minTime = 8f;
    float stone_maxTime = 16f;

    public GameObject stoneFactory;

    public int stonepoolSize = 1;
    public List<GameObject> stoneObjectPool;
    public Transform[] stonespawnPoint;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        aabb = GameObject.Find("CollisionManager").GetComponent<AABB>();


        //stone 복제
        stone_createTime = Random.Range(stone_minTime, stone_maxTime);

        aabb.enemy2 = new Transform[stonepoolSize];
        stoneObjectPool = new List<GameObject>();
        for (int i = 0; i < stonepoolSize; i++)
        {
            GameObject stone = Instantiate(stoneFactory);
            stoneObjectPool.Add(stone);
            stone.SetActive(false);
            
            aabb.enemy2[i] = stone.transform;
        }
    }

    public void Stone2Spawner()
    {
        if (!gameManager.isCounting)
        {
            stone_currentTime += Time.deltaTime; // 현재시간을 측정

            if (stone_currentTime > stone_createTime) // 현재 시간이 돌 생성 시간을 넘으면
            {
                if (stoneObjectPool.Count > 0)
                {
                    GameObject stone = stoneObjectPool[0];
                    stoneObjectPool.Remove(stone);
                    stone.SetActive(true); // 돌을 활성화

                    if (stonespawnPoint.Length > 0)
                    {
                        int index = Random.Range(0, stonespawnPoint.Length);
                        stone.transform.position = stonespawnPoint[index].position;
                    }
                }

                stone_currentTime = 0;
                stone_createTime = Random.Range(stone_minTime, stone_maxTime);
            }
        }
    }
}