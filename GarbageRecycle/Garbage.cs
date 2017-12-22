using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARPG.Common;
using System;

public class Garbage : MonoBehaviour 
{
    private bool isDestroy;
    private Rigidbody2D rb;
    private BoxCollider2D col;

    public GarbageType type;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        col = transform.GetComponent<BoxCollider2D>();
    }

    public void Init()
    {
        isDestroy = false;
        //rb.isKinematic = false;
        rb.gravityScale = 1;
        //col.isTrigger = false;
    }

    public void PickOrDrop()
    {
        //rb.isKinematic = true;
        rb.gravityScale = 0;
        //col.isTrigger = true;
        isDestroy = true;
    }

    public void SetPosition()
    {
        transform.localPosition = Vector3.zero;
    }

    public void RecycleGarbage()
    {
        PickOrDrop();
        transform.parent = GameObject.Find("Canvas/GamePanel(Clone)").transform;
        GameObjectPool.Instance.MyDestory(this.gameObject);
    }

    private void Update()
    {
        if (!isDestroy && transform.localPosition.y <= -1090)
        {
            RecycleGarbage();
        }
    }
}
