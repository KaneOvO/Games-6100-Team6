using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float moveSpeed = 5f;         // 移动速度
    public float rotationSpeed = 200f;   // 旋转速度
    private Rigidbody2D playerRb;        // 2D刚体组件

    void Start()
    {
        // 获取玩家的Rigidbody2D组件
        playerRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        MovementInput();
        RotationInput();
    }

    void MovementInput()
    {
        // 检查是否按下前进键（W或上箭头键）
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            // 添加一个沿着玩家前方向的力
            playerRb.AddRelativeForce(Vector2.up * moveSpeed * Time.deltaTime);
        }
    }

    void RotationInput()
    {
        // 检查是否按下左转键（A或左箭头键）
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            // 逆时针旋转
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
        // 检查是否按下右转键（D或右箭头键）
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            // 顺时针旋转
            transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
        }
    }
}
