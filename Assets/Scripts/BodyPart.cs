using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class BodyPart : MonoBehaviour
{
    // 伤害类型为slicing时使用的精灵
    public Sprite detachedSprite;
    // 伤害类型为burning时使用的精灵
    public Sprite burnedSprite;

    // 身体上血液喷溅的位置和角度
    public Transform bloodFountainOrigin;

    // 如果为真，移除对象上所有的碰撞体、关节、刚体
    bool detached = false;

    // 从父对象分离身体对象，并改变分离条件和标签
    public void Detach()
    {
        detached = true;

        this.tag = "Untagged";

        transform.SetParent(null, true);
    }

    public void Update()
    {

        // 没有分离条件，直接返回
        if (detached == false)
        {
            return;
        }

        // 是否处于休眠状态
        var rigidbody = GetComponent<Rigidbody2D>();

        if (rigidbody.IsSleeping())
        {
            foreach (Joint2D joint in
GetComponentsInChildren<Joint2D>())
            {
                Destroy(joint);
            }

            foreach (Rigidbody2D body in GetComponentsInChildren<Rigidbody2D>())
            {
                Destroy(body);
            }

            foreach (Collider2D collider in GetComponentsInChildren<Collider2D>())
            {
                Destroy(collider);
            }

            Destroy(this.gameObject);
        }
    }

    // 根据受到的伤害类型切换身体部位的精灵图片
    public void ApplyDamageSprite(Gnome.DamageType damageType)
    {

        Sprite spriteToUse = null;

        switch (damageType)
        {

            case Gnome.DamageType.Burning:
                spriteToUse = burnedSprite;

                break;

            case Gnome.DamageType.Slicing:
                spriteToUse = detachedSprite;

                break;
        }

        if (spriteToUse != null)
        {
            GetComponent<SpriteRenderer>().sprite = spriteToUse;
        }

    }

}