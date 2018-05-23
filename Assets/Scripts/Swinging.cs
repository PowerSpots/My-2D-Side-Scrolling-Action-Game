using UnityEngine;
using System.Collections;

// 
public class Swinging : MonoBehaviour
{
    // 倾斜敏感度
    public float swingSensitivity = 100.0f;

    private Rigidbody2D rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // 使用FixedUpdate来更好地利用物理引擎
    void FixedUpdate()
    {

        // 如果没有刚体组件，销毁组件
        if (rigidBody == null)
        {
            Destroy(this);
            return;
        }

        // 获取输入的X分量
        float swing = InputManager.instance.sidewaysMotion;

        // 创建并计算要对刚体施加的力量
        Vector2 force = new Vector2(swing * swingSensitivity, 0);

        rigidBody.AddForce(force);
    }

}