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

        // 缓存线渲染器组件
        lineRenderer = GetComponent<LineRenderer>();

        // 重置绳索长度
        ResetLength();

    }

    // 移除所有绳节，并创建一个新绳索
    public void ResetLength()
    {

        foreach (GameObject segment in ropeSegments)
        {
            Destroy(segment);

        }

        ropeSegments = new List<GameObject>();

        isDecreasing = false;
        isIncreasing = false;

        CreateRopeSegment();

    }

    //在绳索顶部创建一个新绳节
    void CreateRopeSegment()
    {

        // 实例化新绳节
        GameObject segment = (GameObject)Instantiate(ropeSegmentPrefab,

            this.transform.position, Quaternion.identity);

        // 使新绳节成为绳索的子对象
        segment.transform.SetParent(this.transform, true);

        // 获得绳节刚体组件
        Rigidbody2D segmentBody = segment.GetComponent<Rigidbody2D>();

        // 获得弹簧关节
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


        // 如果插入的绳节是第一个，连接矮人
        if (ropeSegments.Count == 1)
        {

            SpringJoint2D connectedObjectJoint =
                connectedObject.GetComponent<SpringJoint2D>();

            connectedObjectJoint.connectedBody = segmentBody;
            connectedObjectJoint.distance = 0.1f;

            segmentJoint.distance = maxRopeSegmentLength;
        }

        // 如果插入时还有其他绳节，连接之前的顶部绳索
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

    // 移除绳索并重新设定连接点和固定点
    void RemoveRopeSegment()
    {
        if (ropeSegments.Count < 2)
        {
            return;
        }

        GameObject topSegment = ropeSegments[0];
        GameObject nextSegment = ropeSegments[1];

        SpringJoint2D nextSegmentJoint =
            nextSegment.GetComponent<SpringJoint2D>();

        nextSegmentJoint.connectedBody =
            this.GetComponent<Rigidbody2D>();


        ropeSegments.RemoveAt(0);
        Destroy(topSegment);

    }

    void Update()
    {

        GameObject topSegment = ropeSegments[0];
        SpringJoint2D topSegmentJoint =
            topSegment.GetComponent<SpringJoint2D>();
        //伸长绳索，超过单段绳节长度则增加一段绳节
        if (isIncreasing)
        {


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
        // 收起绳索，超过单段绳节长度则减少一段绳节
        if (isDecreasing)
        {

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
        // 渲染绳索，渲染的点包括绳索两端的两个点
        if (lineRenderer != null)
        {

            lineRenderer.positionCount = ropeSegments.Count + 2;
            // 渲染的顶点永远是绳索的顶点
            lineRenderer.SetPosition(0, this.transform.position);
            //渲染绳索每个点
            for (int i = 0; i < ropeSegments.Count; i++)
            {
                lineRenderer.SetPosition(i + 1,
                    ropeSegments[i].transform.position);
            }
            //最后一个点是绳索连接物体的点
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
