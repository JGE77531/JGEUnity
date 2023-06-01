using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceManager : MonoBehaviour
{
    public static GameManager gameManager;

    //Ice 복제
    float Ice_currentTime;
    float Ice_minTime = 4f;
    float Ice_maxTime = 11f;

    public float Ice_createTime = 1;
    public GameObject IceFactory;

    public int IcepoolSize = 3;
    public List<GameObject> IceObjectPool;
    public Transform[] IcespawnPoint;

    void Start()
    {
        Ice_createTime = Random.Range(Ice_minTime, Ice_maxTime);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //Ice 복제
        IceObjectPool = new List<GameObject>();
        for (int i = 0; i < IcepoolSize; i++)
        {
            GameObject Ice = Instantiate(IceFactory);
            IceObjectPool.Add(Ice);
            Ice.SetActive(false);
        }
    }

    public void IceSpawner()
    {
        if (!gameManager.isCounting)
        {
            Ice_currentTime += Time.deltaTime; // 현재시간을 측정

            if (Ice_currentTime > Ice_createTime) // 현재 시간이 돌 생성 시간을 넘으면
            {
                if (IceObjectPool.Count > 0)
                {
                    GameObject iceObject = IceObjectPool[0]; // 변수명을 Ice에서 iceObject로 변경
                    IceObjectPool.Remove(iceObject);
                    iceObject.SetActive(true); // 돌을 활성화

                    if (IcespawnPoint.Length > 0)
                    {
                        int index = Random.Range(0, IcespawnPoint.Length);
                        iceObject.transform.position = IcespawnPoint[index].position;
                    }
                }

                Ice_currentTime = 0;
                Ice_createTime = Random.Range(Ice_minTime, Ice_maxTime);
            }
        }
    }
}
