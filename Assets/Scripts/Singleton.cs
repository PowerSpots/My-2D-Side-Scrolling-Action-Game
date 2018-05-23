using UnityEngine;
using System.Collections;

// 这个类允许其他类继承此模板类，并获得一个名为instance的静态属性。该属性将始终指向此类的共享实例。
// GameManager和InputManager类使用这个模板类
// 要使用这个这个类，请用如下形式：
// public class MyManager : Singleton<MyManager>  {  }

// 你可以这么调用：
// MyManager.instance.DoSomething();

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    // 这个类的静态实例
    private static T _instance;

    // 只读属性，返回类的静态实例
    public static T instance
    {
        get
        {
            // 如果还没有设置实例
            if (_instance == null)
            {
                // 尝试搜索这个类
                _instance = FindObjectOfType<T>();
                // 如果没有找到，报错
                if (_instance == null)
                {
                    Debug.LogError("Can't find " + typeof(T) + "!");
                }
            }

            return _instance;
        }
    }

}
