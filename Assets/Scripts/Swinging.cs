using UnityEngine;
using System.Collections;

// 
public class Swinging : MonoBehaviour
{

    public float swingSensitivity = 100.0f;

    // 使用FixedUpdate更好地利用物理引擎
    void FixedUpdate()
    {

        // 如果没有刚体组件，销毁脚本
        if (GetComponent<Rigidbody2D>() == null)
        {
            Destroy(this);
            return;
        }

        // 获取输入的X分量
        float swing = InputManager.instance.sidewaysMotion;

        // 计算对刚体施加的力
        Vector2 force = new Vector2(swing * swingSensitivity, 0);

        GetComponent<Rigidbody2D>().AddForce(force);
    }

}