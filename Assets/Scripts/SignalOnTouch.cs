using UnityEngine;
using System.Collections;

using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class SignalOnTouch : MonoBehaviour
{

    // 当产生碰撞时要触发的事件
    public UnityEvent onTouch;

    public bool playAudioOnTouch = true;

    void OnTriggerEnter2D(Collider2D collider)
    {
        SendSignal(collider.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        SendSignal(collision.gameObject);
    }

    // 如果接触到的对象标签是Player，调用事件
    void SendSignal(GameObject objectThatHit)
    {
        if (objectThatHit.CompareTag("Player"))
        {
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