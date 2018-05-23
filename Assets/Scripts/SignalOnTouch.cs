using UnityEngine;
using System.Collections;
using UnityEngine.Events;

// 当玩家碰到拥有这个组件的对象时调用UnityEvent
[RequireComponent(typeof(Collider2D))]
public class SignalOnTouch : MonoBehaviour
{

    // 碰撞时运行的UnityEvent，要调用的具体方法在编辑器中编辑
    public UnityEvent onTouch;
    // 如果为真，在碰撞时尝试播放音频
    public bool playAudioOnTouch = true;

    // 进入触发区域时，调用SendSignal
    void OnTriggerEnter2D(Collider2D collider)
    {
        SendSignal(collider.gameObject);
    }

    // 碰到对象时，调用SendSignal
    void OnCollisionEnter2D(Collision2D collision)
    {
        SendSignal(collision.gameObject);
    }

    // 检查该对象是否被标记为Player，如果是，则调用UnityEvent
    void SendSignal(GameObject objectThatHit)
    {
        if (objectThatHit.CompareTag("Player"))
        {
            // 如果应该播放声音且具有音源组件的对象处于活动状态，播放声音
            if (playAudioOnTouch)
            {
                var audio = GetComponent<AudioSource>();

                if (audio && audio.gameObject.activeInHierarchy)
                    audio.Play();
            }
            onTouch.Invoke();
        }
    }
}