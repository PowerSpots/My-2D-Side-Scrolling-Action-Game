using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float topLimit = 10.0f;

    public float bottomLimit = -10.0f;

    public float followSpeed = 0.5f;

    // 所有物体计算完位置后，再计算相机应在的位置
    void LateUpdate()
    {

        if (target != null)
        {

            // 获得相机位置
            Vector3 newPosition = this.transform.position;

            // 匹配它所连接对象的变换的Y位置
            newPosition.y = Mathf.Lerp(newPosition.y,
target.position.y, followSpeed);
            // 确保该位置不超过或低于特定阈值
            newPosition.y = Mathf.Min(newPosition.y, topLimit);
            newPosition.y = Mathf.Max(newPosition.y, bottomLimit);

            transform.position = newPosition;
        }



    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 topPoint = new Vector3(this.transform.position.x,
topLimit, this.transform.position.z);
        Vector3 bottomPoint = new Vector3(this.transform.position.x,
bottomLimit, this.transform.position.z);

        Gizmos.DrawLine(topPoint, bottomPoint);
    }
}
