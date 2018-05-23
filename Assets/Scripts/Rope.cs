using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rope : MonoBehaviour
{
    // 单段绳节的预制件
    public GameObject ropeSegmentPrefab;

    // 包含单段绳节的链表
    List<GameObject> ropeSegments = new List<GameObject>();

    // 伸长或者收缩绳索
    public bool isIncreasing { get; set; }
    public bool isDecreasing { get; set; }

    // 每段绳节上的刚体组件
    public Rigidbody2D connectedObject;

    // 每段绳节的最大伸长量
    public float maxRopeSegmentLength = 1.0f;

    // 绳索速度
    public float ropeSpeed = 4.0f;

    // 绳索的线渲染器组件
    LineRenderer lineRenderer;

    void Start()
    {

        // 线渲染器变量设置为指向附加到该对象的线渲染器组件
        lineRenderer = GetComponent<LineRenderer>();

        // 重置绳索长度
        ResetLength();

    }

    // 移除所有绳节，并创建一个新绳索
    public void ResetLength()
    {
        // 删除所有绳节并清空绳索列表
        foreach (GameObject segment in ropeSegments)
        {
            Destroy(segment);

        }
        ropeSegments = new List<GameObject>();

        // 重置内部状态
        isDecreasing = false;
        isIncreasing = false;

        CreateRopeSegment();

    }

    // 在绳索顶部创建一个新绳节
    void CreateRopeSegment()
    {

        // 实例化新绳节
        GameObject segment = (GameObject)Instantiate(ropeSegmentPrefab,

            this.transform.position, Quaternion.identity);

        // 使新绳节成为绳索的子对象
        segment.transform.SetParent(this.transform, true);

        // 获得绳节起始点刚体组件
        Rigidbody2D segmentBody = segment.GetComponent<Rigidbody2D>();

        // 获得绳节的弹簧关节
        SpringJoint2D segmentJoint =
            segment.GetComponent<SpringJoint2D>();

        if (segmentBody == null || segmentJoint == null)
        {
            Debug.LogError("Rope segment body prefab has no " +
                "Rigidbody2D and/or SpringJoint2D!");
            return;
        }

        // 如果绳节不为空，插入绳索顶部
        ropeSegments.Insert(0, segment);


        // 如果插入的绳节是第一个，将自己连接到设定的ConnectedObject刚体（矮人的腿部）
        if (ropeSegments.Count == 1)
        {
            // 获得矮人腿部的关节组件并连接到第一段绳节
            SpringJoint2D connectedObjectJoint =
                connectedObject.GetComponent<SpringJoint2D>();
            connectedObjectJoint.connectedBody = segmentBody;
            connectedObjectJoint.distance = 0.1f;
            // 此时绳节已经达到最大长度
            segmentJoint.distance = maxRopeSegmentLength;
        }

        // 如果插入时还有其他绳节，将已经存在的绳索部分连接到新创建的这段 
        else
        {

            GameObject nextSegment = ropeSegments[1];

            SpringJoint2D nextSegmentJoint =
                nextSegment.GetComponent<SpringJoint2D>();

            nextSegmentJoint.connectedBody = segmentBody;

            segmentJoint.distance = 0.0f;
        }
        // 连接整段绳索到固定点
        segmentJoint.connectedBody = this.GetComponent<Rigidbody2D>();
    }

    // 移除绳节时调用，并重新设定连接点和固定点
    void RemoveRopeSegment()
    {
        // 少于两段绳节时返回
        if (ropeSegments.Count < 2)
        {
            return;
        }

        GameObject topSegment = ropeSegments[0];
        GameObject nextSegment = ropeSegments[1];
        // 连接第二段绳节到固定点
        SpringJoint2D nextSegmentJoint =
            nextSegment.GetComponent<SpringJoint2D>();
        nextSegmentJoint.connectedBody =
            this.GetComponent<Rigidbody2D>();

        // 移除顶端绳节
        ropeSegments.RemoveAt(0);
        Destroy(topSegment);

    }

    void Update()
    {
        // 获得顶部绳节和弹簧组件
        GameObject topSegment = ropeSegments[0];
        SpringJoint2D topSegmentJoint =
            topSegment.GetComponent<SpringJoint2D>();
        // 伸长绳索，超过单段绳节最大长度则增加一段绳节
        if (isIncreasing)
        {

            // 增加绳索，如果绳节已经处于最大长度，则添加一个新绳节
            // 否则增加顶部绳节的长度，直到达到这段绳节的最大长度
            if (topSegmentJoint.distance >= maxRopeSegmentLength)
            {
                CreateRopeSegment();
            }
            else
            {
                topSegmentJoint.distance += ropeSpeed *
                    Time.deltaTime;
            }

        }
        // 收起绳索，长度接近于零则减少一段绳节
        if (isDecreasing)
        {
            // 减少绳索，如果它接近零长度，则删除绳节; 否则，减小顶部绳节的长度。
            if (topSegmentJoint.distance <= 0.005f)
            {
                RemoveRopeSegment();
            }
            else
            {
                topSegmentJoint.distance -= ropeSpeed *
                    Time.deltaTime;
            }

        }
        // 线渲染器从一组点中绘制线条，这些点需要与绳段的位置保持同步。 
        // 线渲染器顶点的数量 = 绳索段的数量 + 2（绳索锚点顶部的点 + 底部侏儒上的点）
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = ropeSegments.Count + 2;

            // 渲染的顶点永远是绳索顶部的锚点
            lineRenderer.SetPosition(0, this.transform.position);
            // 使线渲染器组件的顶点一一对应位于每个绳节段
            for (int i = 0; i < ropeSegments.Count; i++)
            {
                lineRenderer.SetPosition(i + 1,
                    ropeSegments[i].transform.position);
            }
            // 最后一点是连接对象的锚点
            SpringJoint2D connectedObjectJoint =
                connectedObject.GetComponent<SpringJoint2D>();
            lineRenderer.SetPosition(
                ropeSegments.Count + 1,
                connectedObject.transform.
                    TransformPoint(connectedObjectJoint.anchor)
            );
        }
    }

}
