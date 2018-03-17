using UnityEngine;
using System.Collections;

// 输入管理类将加速计数据转换为横向运动信息
public class InputManager : Singleton<InputManager>
{

    // 移动距离. -1.0最左, +1.0最右
    private float _sidewaysMotion = 0.0f;

    // 存储加速度计X坐标分量的只读属性
    public float sidewaysMotion
    {
        get
        {
            return _sidewaysMotion;
        }
    }
 
    void Update()
    {
        Vector3 accel = Input.acceleration;

        _sidewaysMotion = accel.x;

    }
}
