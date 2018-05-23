using UnityEngine;
using System.Collections;
/// <summary>
/// 在一定范围内调整摄像头，来始终跟随目标对象的Y轴坐标位置
/// </summary>
public class CameraFollow : MonoBehaviour
{
    // 我们想要跟随的对象位置
    public Transform target;

    // 相机可以到达的最高点和最低点
    public float topLimit = 11.5f;
    public float bottomLimit = -10.7f;

    // 跟随速度
    public float followSpeed = 0.5f;

    // 计算完所有物体的位置后，再计算相机应在的位置
    void LateUpdate()
    {
        if (target != null)
        {
            // 获得当前相机位置
            Vector3 newPosition = this.transform.position;

            // 通过相机和追随对象的插值计算，得出相机应在的y轴位置
            newPosition.y = Mathf.Lerp(newPosition.y,
target.position.y, followSpeed);
            // 确保y轴位置不超过或低于特定阈值
            newPosition.y = Mathf.Min(newPosition.y, topLimit);
            newPosition.y = Mathf.Max(newPosition.y, bottomLimit);

            transform.position = newPosition;
        }
    }

    // 在编辑器中被选中时，绘制一条从顶部到底部的黄色线段使相机运动范围可视化
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
