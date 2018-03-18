using UnityEngine;
using System.Collections;

public class SpriteSwapper : MonoBehaviour
{
    public Sprite spriteToUse;

    // 要交换精灵图片的对象的精灵渲染器
    public SpriteRenderer spriteRenderer;

    // 原来的精灵图片，用来重设精灵图片
    private Sprite originalSprite;

    public void SwapSprite()
    {

        // 如果要使用的精灵图片和现在的精灵不同，发生交换
        if (spriteToUse != spriteRenderer.sprite)
        {
            //保存初始的精灵图片
            originalSprite = spriteRenderer.sprite;

            spriteRenderer.sprite = spriteToUse;
        }
    }

    // 重设会初始精灵图片
    public void ResetSprite()
    {

        // 如果初始精灵图存在
        if (originalSprite != null)
        {
            spriteRenderer.sprite = originalSprite;
        }
    }
}