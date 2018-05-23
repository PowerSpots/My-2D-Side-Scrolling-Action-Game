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

    // 从父对象分离身体对象，并改变分离条件和标签
    public void Detach()
    {
        this.tag = "Untagged";

        transform.SetParent(null, true);
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