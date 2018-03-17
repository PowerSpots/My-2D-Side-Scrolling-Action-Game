using UnityEngine;
using System.Collections;


public class GameManager : Singleton<GameManager>
{

    // 矮人的生成点
    public GameObject startingPoint;

    public Rope rope;

    public CameraFollow cameraFollow;

    // 游戏中活动的当前矮人
    Gnome currentGnome;

    public GameObject gnomePrefab;

    // 包含“重新开始”和“恢复”的UI组件
    public RectTransform mainMenu;

    // 包含上、下按钮的UI组件
    public RectTransform gameplayMenu;

    // 包含“胜利！”的UI组件
    public RectTransform gameOverMenu;

    // 无敌属性
    // 如果为真，进入无敌状态，忽视所有伤害，但仍显示伤害效果
    public bool gnomeInvincible { get; set; }

    public float delayAfterDeath = 1.0f;

    public AudioClip gnomeDiedSound;

    public AudioClip gameOverSound;

    void Start()
    {
        // 游戏开始时重设所有状态
        Reset();
    }

    public void Reset()
    {

        // 关闭其他菜单，打开游戏开始菜单
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

        // 正常速度开始游戏
        Time.timeScale = 1.0f;
    }

    void CreateNewGnome()
    {

        // 移除之前存在的矮人对象
        RemoveGnome();

        // 实例化一个新矮人
        GameObject newGnome = (GameObject)Instantiate(gnomePrefab,
            startingPoint.transform.position, Quaternion.identity);
        currentGnome = newGnome.GetComponent<Gnome>();

        // 使绳索可见
        rope.gameObject.SetActive(true);

        // 连接绳索末端和当前矮人
        rope.connectedObject = currentGnome.ropeBody;

        // 重设绳索长度
        rope.ResetLength();

        // 相机开始跟随目标对象
        cameraFollow.target = currentGnome.cameraFollowTarget;

    }

    void RemoveGnome()
    {

        // 无敌状态不能移除矮人
        if (gnomeInvincible)
            return;

        // 隐藏绳索
        rope.gameObject.SetActive(false);

        // 相机停止跟随矮人
        cameraFollow.target = null;

        // 如果之前存在一个矮人，设为空
        if (currentGnome != null)
        {
            currentGnome.holdingTreasure = false;
            //移除Player标签
            currentGnome.gameObject.tag = "Untagged";
            foreach (Transform child in currentGnome.transform)
            {
                child.gameObject.tag = "Untagged";
            }

            currentGnome = null;
        }
    }

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

        // 如果不处于无敌状态，
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

    // 到达出口时，根据玩家是否获得宝藏作为游戏胜利条件
    public void ExitReached()
    {
        // 玩家获得宝藏时，暂停游戏，播放胜利音效和结束菜单
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
        }
    }

    // 暂停游戏并激活暂停主菜单
    public void SetPaused(bool paused)
    {
        if (paused)
        {
            Time.timeScale = 0.0f;
            mainMenu.gameObject.SetActive(true);
            gameplayMenu.gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 1.0f;
            mainMenu.gameObject.SetActive(false);
            gameplayMenu.gameObject.SetActive(true);
        }
    }

    // 重新开始游戏并清空当前矮人对象
    public void RestartGame()
    {
        Destroy(currentGnome.gameObject);
        currentGnome = null;

        Reset();
    }
}