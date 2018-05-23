using UnityEngine;
using System.Collections;

/// <summary>
/// 管理游戏状态
/// </summary>
public class GameManager : Singleton<GameManager>
{

    // 矮人的生成点
    public GameObject startingPoint;
    // 降低或拉升矮人的绳子
    public Rope rope;
    // 相机跟随矮人的脚本
    public CameraFollow cameraFollow;
    // 游戏中存活的当前矮人
    Gnome currentGnome;
    // 需要一个新矮人时实例化的预制件
    public GameObject gnomePrefab;

    // 包含“重新开始”和“恢复”的UI组件
    public RectTransform mainMenu;
    // 包含上、下按钮的UI组件
    public RectTransform gameplayMenu;
    // 包含“胜利！”的UI组件
    public RectTransform gameOverMenu;

    // 无敌状态，测试用
    // 如果为真，进入无敌状态，忽视所有伤害，但仍显示伤害效果
    public bool gnomeInvincible { get; set; }
    // 创建新矮人前等待的时间
    public float delayAfterDeath = 3.0f;
    // 矮人死亡时播放的声音
    public AudioClip gnomeDiedSound;
    // 矮人胜利时播放的声音
    public AudioClip gameOverSound;

    void Start()
    {
        // 游戏开始时重设所有状态
        Reset();
    }
    // 重设整个游戏
    public void Reset()
    {

        // 关闭其他菜单，打开游戏中菜单
        if (gameOverMenu)
            gameOverMenu.gameObject.SetActive(false);
        if (mainMenu)
            mainMenu.gameObject.SetActive(false);
        if (gameplayMenu)
            gameplayMenu.gameObject.SetActive(true);

        // 搜索所有可重设对象，并循环遍历重设
        var resetObjects = FindObjectsOfType<Resettable>();
        foreach (Resettable r in resetObjects)
        {
            r.Reset();
        }

        // 生成新矮人
        CreateNewGnome();

        // 正常速度开始游戏防止游戏暂停
        Time.timeScale = 1.0f;
    }

    void CreateNewGnome()
    {

        // 移除已经存在的矮人对象
        RemoveGnome();

        // 实例化一个新矮人，替代我们的当前矮人
        GameObject newGnome = (GameObject)Instantiate(gnomePrefab,
            startingPoint.transform.position, Quaternion.identity);
        currentGnome = newGnome.GetComponent<Gnome>();

        // 使绳索可见
        rope.gameObject.SetActive(true);

        // 连接绳索末端和当前矮人的脚踝
        rope.connectedObject = currentGnome.ropeBody;

        // 重设绳索长度为默认值
        rope.ResetLength();

        // 相机开始跟随新生成的目标对象
        cameraFollow.target = currentGnome.cameraFollowTarget;

    }
    // 移除矮人
    void RemoveGnome()
    {

        // 无敌状态直接返回
        if (gnomeInvincible)
            return;

        // 禁用绳索
        rope.gameObject.SetActive(false);

        // 相机停止跟随矮人
        cameraFollow.target = null;

        // 如果之前存在一个矮人，切回普通精灵图片，移除Player标签并当前矮人设为空
        if (currentGnome != null)
        {
            // 不再持有宝藏
            currentGnome.holdingTreasure = false;
            // 移除整体和各部分的Player标签，和陷阱产生碰撞时不再通知游戏管理器
            currentGnome.gameObject.tag = "Untagged";
            foreach (Transform child in currentGnome.transform)
            {
                child.gameObject.tag = "Untagged";  
            }
            currentGnome = null;
        }
    }

    // 杀死矮人
    void KillGnome(Gnome.DamageType damageType)
    {
        // 播放死亡音效
        var audio = GetComponent<AudioSource>();
        if (audio)
        {
            audio.PlayOneShot(this.gnomeDiedSound);
        }

        // 显示伤害效果
        currentGnome.ShowDamageEffect(damageType);

        // 如果不处于无敌状态
        if (gnomeInvincible == false)
        {

            // 根据伤害类型摧毁矮人
            currentGnome.DestroyGnome(damageType);

            // 移除矮人
            RemoveGnome();

            // 一段时间后重设游戏
            StartCoroutine(ResetAfterDelay());
        }
    }

    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(delayAfterDeath);

        Reset();
    }

    // 遇到陷阱，触发切割伤害
    public void TrapTouched()
    {
        KillGnome(Gnome.DamageType.Slicing);
    }

    // 遇到火球，触发灼烧伤害
    public void FireTrapTouched()
    {
        KillGnome(Gnome.DamageType.Burning);
    }

    // 拿到宝藏后将收集宝藏属性设为真
    public void TreasureCollected()
    {       
        currentGnome.holdingTreasure = true;
    }

    // 到达出口时，根据玩家是否存活和获得宝藏作为游戏胜利条件
    public void ExitReached()
    {
        // 玩家获得宝藏且没有死亡时，播放胜利音效，暂停游戏，显示结束菜单，关闭游戏中菜单
        if (currentGnome != null && currentGnome.holdingTreasure == true)
        {
            var audio = GetComponent<AudioSource>();
            if (audio)
            {
                audio.PlayOneShot(this.gameOverSound);
            }

            Time.timeScale = 0.0f;

            if (gameOverMenu)
                gameOverMenu.gameObject.SetActive(true);
            if (gameplayMenu)
                gameplayMenu.gameObject.SetActive(false);

            gnomeInvincible = false;
        }
    }

    // 点击菜单按钮和点击恢复游戏按钮时调用
    public void SetPaused(bool paused)
    {
        if (paused)
        {
            // 暂停游戏并激活暂停菜单
            Time.timeScale = 0.0f;
            mainMenu.gameObject.SetActive(true);
            gameplayMenu.gameObject.SetActive(false);
        }
        else
        {
            // 恢复游戏并激活游戏中菜单
            Time.timeScale = 1.0f;
            mainMenu.gameObject.SetActive(false);
            gameplayMenu.gameObject.SetActive(true);
        }
    }

    // 重新开始游戏
    public void RestartGame()
    {
        // 清空当前矮人对象
        Destroy(currentGnome.gameObject);
        currentGnome = null;
        // 重新开始游戏
        Reset();
    }
}