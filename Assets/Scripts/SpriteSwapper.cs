using UnityEngine;
using System.Collections;

// 交换两个精灵图片。例如，宝藏从显示宝藏切换到不显示宝藏
public class SpriteSwapper : MonoBehaviour
{
    // 将要显示的精灵
    public Sprite spriteToUse;

    // 新精灵图片的精灵渲染器
    public SpriteRenderer spriteRenderer;

    // 原始精灵图片，用来重设精灵图片
    private Sprite originalSprite;

    // 交换精灵图片
    public void SwapSprite()
    {

        // 如果要使用的精灵图片和现在的精灵不同，发生交换
        if (spriteToUse != spriteRenderer.sprite)
        {
            //保存初始的精灵图片
            originalSprite = spriteRenderer.sprite;
            // 让精灵渲染器使用新的精灵
            spriteRenderer.sprite = spriteToUse;
        }
    }

    // 恢复到旧的精灵图片
    public void ResetSprite()
    {

        // 如果初始精灵图存在，恢复回初始精灵图片
        if (originalSprite != null)
        {
            spriteRenderer.sprite = originalSprite;
        }
    }
}