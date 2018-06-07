using UnityEngine;
using System.Collections;


public class Gnome : MonoBehaviour
{

    // 相机跟随的对象
    public Transform cameraFollowTarget;

    public Rigidbody2D ropeBody;

    public Sprite armHoldingEmpty;
    public Sprite armHoldingTreasure;

    public SpriteRenderer holdingArm;

    public GameObject deathPrefab;
    public GameObject flameDeathPrefab;
    public GameObject ghostPrefab;

    public float delayBeforeRemoving = 3.0f;
    public float delayBeforeReleasingGhost = 0.25f;

    public GameObject bloodFountainPrefab;
    public GameObject smokeExplosionPrefab;


    bool dead = false;

    bool _holdingTreasure = false;
    // 当holdingTreasure属性改变时，通过set方法自动改变手臂精灵
    public bool holdingTreasure
    {
        get
        {
            return _holdingTreasure;
        }
        set
        {
            if (dead == true)
            {
                return;
            }

            _holdingTreasure = value;

            if (holdingArm != null)
            {
                if (_holdingTreasure)
                {
                    holdingArm.sprite = armHoldingTreasure;
                }
                else
                {
                    holdingArm.sprite = armHoldingEmpty;
                }
            }
        }
    }
    // 受到伤害的类型
    public enum DamageType
    {
        Slicing,
        Burning
    }
    // 根据受到伤害的类型创建不同伤害效果的实例
    public void ShowDamageEffect(DamageType type)
    {
        switch (type)
        {
            case DamageType.Burning:
                if (flameDeathPrefab != null)
                {
                    Instantiate(
                        flameDeathPrefab, cameraFollowTarget.position,
                        cameraFollowTarget.rotation);
                }
                break;

            case DamageType.Slicing:
                if (deathPrefab != null)
                {
                    Instantiate(
                        deathPrefab,
                        cameraFollowTarget.position,
                        cameraFollowTarget.rotation
                    );
                }
                break;
        }
    }

    public void DestroyGnome(DamageType type)
    {

        holdingTreasure = false;

        dead = true;

        // 获取所有身体对象并应用效果
        foreach (BodyPart part in GetComponentsInChildren<BodyPart>())
        {

            switch (type)
            {

                case DamageType.Burning:
                    // 三分之一概率的灼烧效果
                    bool shouldBurn = Random.Range(0, 2) == 0;
                    if (shouldBurn)
                    {
                        part.ApplyDamageSprite(type);
                    }
                    break;

                case DamageType.Slicing:
                    // 应用伤害精灵图片
                    part.ApplyDamageSprite(type);

                    break;
            }

            // 二分之一的概率被从身体中分离
            bool shouldDetach = Random.Range(0, 1) == 0;

            if (shouldDetach)
            {
                // 切割伤害类型
                if (type == DamageType.Slicing)
                {

                    if (part.bloodFountainOrigin != null &&
                        bloodFountainPrefab != null)
                    {

                        // 向这个分离部分添加溅血预制体
                        GameObject fountain = Instantiate(
                            bloodFountainPrefab,
                            part.bloodFountainOrigin.position,
                            part.bloodFountainOrigin.rotation
                        ) as GameObject;

                        fountain.transform.SetParent(
                            this.cameraFollowTarget,
                            false
                        );
                    }
                }

                // 从父对象中分离并摧毁关节
                var allJoints = part.GetComponentsInChildren<Joint2D>();
                foreach (Joint2D joints in allJoints)
                {
                    Destroy(joints);
                }
            }
            // 移除这个身体部分
            part.Detach();
        }

        // 通过RemoveAfterDelay组件从整个游戏中移除矮人
        var remove = gameObject.AddComponent<RemoveAfterDelay>();
        remove.delay = delayBeforeRemoving;
    }
}
