using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    // 要加载的场景名称
    public string sceneToLoad;

    // 载入中 文本UI控件
    public RectTransform loadingOverlay;

    // 场景背景加载，用于控制场景切换的时间。
    AsyncOperation sceneLoadingOperation;

    public void Start()
    {

        // 禁用 载入中 文本UI
        loadingOverlay.gameObject.SetActive(false);

        // 开始在后台载入游戏主场景
        sceneLoadingOperation =
SceneManager.LoadSceneAsync(sceneToLoad);

        // 直到主场景准备好之前，并不真正切换到主场景
        sceneLoadingOperation.allowSceneActivation = false;


    }

    // 新游戏 按钮被点击时调用的载入场景方法
    public void LoadScene()
    {

        loadingOverlay.gameObject.SetActive(true);

        // 切换到已经准备好的主场景
        sceneLoadingOperation.allowSceneActivation = true;

    }


}
// END 2d_mainmenu
