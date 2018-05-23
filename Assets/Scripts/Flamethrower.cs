using UnityEngine;
using System.Collections;
using UnityEngine.Events;

// 以特定的速度朝某个方向移动
public class Flamethrower : MonoBehaviour {

    // 发射火球时使用的精灵
    public Sprite activeSprite;

    // 不发射火球时使用的精灵
    public Sprite inactiveSprite;

    // 改变精灵图片的精灵渲染器
    public SpriteRenderer spriteRenderer;

    // 发射火球时创建的对象
    public GameObject fireballPrefab;

    // 发射前等待的时间
    public float timeBetweenShots = 4.0f;

    // 发射火球时精灵图片的显示时长
    public float timeToCoolDown = 0.2f;

    // 火球发射位置
    public Transform emissionPoint;

    // 第一次发射火球前等待的时间
    public float timeToStart = 1.0f;

    // 发射火球时播放的声音
    public AudioClip fireballSound;

	void Start () {
        // 确保正在使用不活动的精灵
        spriteRenderer.sprite = inactiveSprite;
    
        // 开始射击火球的协程
        StartCoroutine("ShootFireballs");
	}

	IEnumerator ShootFireballs() {

        // 等待timeToStart秒开始发射
        yield return new WaitForSeconds(timeToStart);

        // 永远循环
        while (true) {

            // 交换两种状态的精灵图片，然后在timeToCoolDown秒之后重置
            StartCoroutine("Cooldown");

			// 确保火球不为空
			if (fireballPrefab != null) {

				// 如果有音源，播放
				var audio = GetComponent<AudioSource>();
				if (audio && fireballSound) {
					audio.PlayOneShot(fireballSound);
				}

                // 在发射点的位置创建火球，但没有旋转
                var fireball = 
                    (GameObject)Instantiate(
                        fireballPrefab, 
                        emissionPoint.position, 
                        Quaternion.identity
                    );

                // 使火球的移动组件在发射器对象的向右方向移动（红色箭头）
                fireball.GetComponent<Mover>().direction =
                    transform.right;

                // 将火球的Signal On Touch连接到游戏管理器
                fireball.GetComponent<SignalOnTouch>().
                    onTouch.AddListener(
    					delegate {
                            // 当火球碰到矮人时，通知FireTrapTouched杀死矮人
                            GameManager.instance.
                                FireTrapTouched(); 
    					}
    				);
			}

            // 等待后再次发射
            yield return new WaitForSeconds(this.timeBetweenShots);


		}
	}

	IEnumerator Cooldown() {
        // 启用activeSprite
        spriteRenderer.sprite = activeSprite;

        // 等待timeToCoolDown秒
        yield return new WaitForSeconds(timeToCoolDown);

        // 交换回inactiveSprite精灵图片
        spriteRenderer.sprite = inactiveSprite;
	}

}
