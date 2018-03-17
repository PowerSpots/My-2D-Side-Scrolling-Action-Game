using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class BodyPart : MonoBehaviour
{
    public Sprite detachedSprite;

    public Sprite burnedSprite;

    // 躯干上血液喷溅的位置和角度
    public Transform bloodFountainOrigin;

    // 如果分离条件为真，躯干会被移除碰撞体、关节和刚体
    bool detached = false;

    // 从父对象分离躯干对象，并改变分离条件和标签
    public void Detach()
    {
        detached = true;

        this.tag = "Untagged";

        transform.SetParent(null, true);
    }

    public void Update()
    {
        if (detached == false)
        {
            return;
        }

        var rigidbody = GetComponent<Rigidbody2D>();
        // 刚体被认休眠状态时，移除所有关节、刚体、碰撞体
        if (rigidbody.IsSleeping())
        {
            foreach (Joint2D joint in GetComponentsInChildren<Joint2D>())
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

            Destroy(this);
        }
    }

    // 根据受到伤害的类型切换躯干的精灵图片
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