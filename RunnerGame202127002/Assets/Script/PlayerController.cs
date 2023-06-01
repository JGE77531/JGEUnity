using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;

    public float jumpForce = 11f; // 점프 힘
    public float gravity = 20f; // 중력 가속도

    private float verticalVelocity = 0f; // 수직 속도
    private Vector2 startPos;
    private bool Isjump = true;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        startPos = transform.position;
    }

    void Update()
    {
        if (gameManager.state == GameState.Ready)
        {
            transform.position = startPos;
            Isjump = true;
        }

        if (transform.position.y <= -2.15f)
        {
            transform.position = startPos;
            Isjump = true;
        }

        ApplyGravity(); // 중력 적용 및 이동
    }

    private void Jump()
    {
        gravity = 20f;
        if (!gameManager.isCounting && Isjump)
        {
            Isjump = false;
            verticalVelocity = jumpForce;
        }
    }

    private void ApplyGravity()
    {
        if (transform.position.y <= -2.15f)
        {
            gravity = 0;
        }
        else
        {
            gravity = 20f;
        }
        verticalVelocity -= gravity * Time.deltaTime;
        transform.position += new Vector3(0f, verticalVelocity * Time.deltaTime, 0f);


        if (transform.position.y <= -2.15f)
        {
            transform.position = startPos;
        }
    }
}