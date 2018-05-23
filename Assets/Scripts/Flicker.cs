using UnityEngine;
using System.Collections;

// 随机更改精灵渲染器的alpha分量
[RequireComponent(typeof(SpriteRenderer))]
public class Flicker : MonoBehaviour {

    // 最小的alpha值
    [Range(0.0f, 1.0f)]
	public float minimumBrightness = 0.5f;

    // 最大的alpha值
    [Range(0.0f, 1.0f)]
	public float maximumBrightness = 1.0f;

    // 应该有多少光的闪烁。0 = 没有闪烁，1 = 很多闪烁
    [Range(0.0f, 1.0f)]
	public float flickerStrength = 0.1f;

	void Update () {

		// 获得精灵渲染器
		var spriteRenderer = GetComponent<SpriteRenderer>();

		// 计算出新的alpha值
		var flickerAmount = Random.Range(minimumBrightness, 
maximumBrightness);

		var color = spriteRenderer.color;

        // 使用闪烁强度flickerStrength来计算flickerAmount应该影响当前alpha值的数量。
        // 1.0 = 全部，0.0 = 无。

        color.a *= 1.0f - flickerStrength;
		color.a += flickerAmount * flickerStrength;

		// 应用新的alpha值
		spriteRenderer.color = color;
	}
}
