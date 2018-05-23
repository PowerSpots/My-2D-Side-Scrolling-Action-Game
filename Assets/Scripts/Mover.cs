using UnityEngine;
using System.Collections;

// 以特定的速度朝某个方向移动
public class Mover : MonoBehaviour {

    // 每秒移动的速度
    public float speed = 6.0f;

    // 在世界空间的移动方向
    public Vector3 direction;

	void Update () {

        // 每一帧都以给定的速度朝给定的方向移动
        transform.Translate(direction * speed * Time.deltaTime);
	}
}
