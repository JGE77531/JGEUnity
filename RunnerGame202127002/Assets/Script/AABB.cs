using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABB : MonoBehaviour
{
    private GameManager gameManager;
    public Transform player;
    public Transform[] enemy1;
    public Transform[] enemy2;

    private bool hasDied = false; // �̹� DieEvent�� ȣ��Ǿ����� ���θ� �����ϴ� ����

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (gameManager.state == GameState.Ready)
        {
            hasDied = false;
        }

        if (!hasDied && IsAABBCollision(player, enemy1, enemy2)) // hasDied�� false�̰� �浹�� �����Ǹ�
        {
            if (gameManager.state == GameState.Play)
            {
                DieEvent();
                hasDied = true; // DieEvent�� ȣ��Ǿ����� ǥ��
            }
        }
    }

    public bool IsAABBCollision(Transform player, Transform[] enemy1, Transform[] enemy2)
    {
        Vector2 playerMin = player.position - player.localScale / 20f;
        Vector2 playerMax = player.position + player.localScale / 20f;

        foreach (Transform enemy in enemy1)
        {
            if (enemy.gameObject.activeSelf)
            {
                Vector2 enemyMin = enemy.position - enemy.localScale / 20f;
                Vector2 enemyMax = enemy.position + enemy.localScale / 20f;

                if (playerMin.x <= enemyMax.x && playerMax.x >= enemyMin.x &&
                    playerMin.y <= enemyMax.y && playerMax.y >= enemyMin.y)
                {
                    return true;
                }
            }
        }

        foreach (Transform enemy in enemy2)
        {
            if (enemy.gameObject.activeSelf)
            {
                Vector2 enemyMin = enemy.position - enemy.localScale / 20f;
                Vector2 enemyMax = enemy.position + enemy.localScale / 20f;

                if (playerMin.x <= enemyMax.x && playerMax.x >= enemyMin.x &&
                    playerMin.y <= enemyMax.y && playerMax.y >= enemyMin.y)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void DieEvent()
    {
        gameManager.state = GameState.End;
    }
}