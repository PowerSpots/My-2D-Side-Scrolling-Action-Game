using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Resettable : MonoBehaviour
{

    // 将游戏重置时需要运行的方法与事件连接
    public UnityEvent onReset;

    // 游戏重置时由游戏管理器调用
    public void Reset()
    {
        // 调用事件，并执行事件中所有的方法和属性更改。
        onReset.Invoke();
    }
}