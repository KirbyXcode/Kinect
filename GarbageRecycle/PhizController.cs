using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopEngine;
using ARPG.Common;
using DG.Tweening;

public class PhizController : MonoSingleton<PhizController> 
{
    private GameObject good;
    private GameObject bad;
    private Transform phizRootTrans;

    private void Awake()
    {
        good = Factory.Instance.LoadGameObject(Defines.Good);
        bad = Factory.Instance.LoadGameObject(Defines.Bad);
        Transform canvasTrans = GameObject.FindGameObjectWithTag(Defines.Tag_Canvas).transform;
        phizRootTrans = Global.FindChild(canvasTrans, "PhizRoot").transform;
    }

    public void CreateGood(Transform target)
    {
        GameObject go = GameObjectPool.Instance.CreateObject(Defines.Pool_PhizGood, good, Vector3.zero, Quaternion.identity);
        SetPosition(go, target);
    }

    public void CreateBad(Transform target)
    {
        GameObject go = GameObjectPool.Instance.CreateObject(Defines.Pool_PhizBad, bad, Vector3.zero, Quaternion.identity);
        SetPosition(go, target);
    }

    private void SetPosition(GameObject go, Transform target)
    {
        go.transform.parent = phizRootTrans;
        go.transform.localScale = Vector3.one;
        Vector3 originalPos = target.localPosition + new Vector3(0, 134, 0);
        Vector3 targetPos = originalPos + new Vector3(0, 122, 0);
        go.transform.localPosition = originalPos;

        PlayAnimation(go, targetPos);
    }

    private void PlayAnimation(GameObject go, Vector3 targetPos)
    {
        CanvasGroup cg = go.GetComponent<CanvasGroup>();
        cg.alpha = 0;

        cg.DOFade(1, 1f).SetEase(Ease.OutQuart);
        go.transform.DOLocalMoveY(targetPos.y, 1.5f).OnComplete(() => GameObjectPool.Instance.MyDestory(go,0.5f));
    }
}
