using UnityEngine;
using System.Collections;

// 输入管理类将加速计数据转换为横向运动信息
public class InputManager : Singleton<InputManager>
{

    // 移动距离，-1.0最左, +1.0最右
    private float _sidewaysMotion = 0.0f;

    // 存储加速度计X坐标的分量，通过只读属性公开
    // 如果其他任何一类想要获得手机沿左右轴倾斜的程度，只需要访问InputManager.instance.sidewaysMotion
    public float sidewaysMotion
    {
        get
        {
            return _sidewaysMotion;
        }
    }
 
    void Update()
    {
        // 通过内置的Input类对来自加速度计的数据进行采样，并将X分量存储在变量中
        Vector3 accel = Input.acceleration;
        _sidewaysMotion = accel.x;

    }
}
