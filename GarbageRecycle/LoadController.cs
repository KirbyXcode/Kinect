using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopEngine;
using UnityEngine.UI;
using EventCenter;

public class LoadController :  MonoSingleton<LoadController>
{
    private GameObject loadMask;
    private Image mImage_LoadMask;
    private bool isLoad;
    private float smooth = 2.5f;
    private ButtonType type;

	public void InitLoadMask()
    {
        loadMask = Factory.Instance.InstGameObject(Defines.LoadMask);
        mImage_LoadMask = loadMask.GetComponent<Image>();
        GameObject canvas = GameObject.FindGameObjectWithTag(Defines.Tag_Canvas);
        loadMask.transform.parent = canvas.transform;
        loadMask.transform.localPosition = Vector3.zero;
        loadMask.transform.localEulerAngles = Vector3.zero;
        loadMask.transform.localScale = Vector3.one;
        ShowOrHide(false);
    }

    public void ShowOrHide(bool isShow)
    {
        if (loadMask != null)
        {
            if(isShow)
            {
                loadMask.SetActive(isShow);
                mImage_LoadMask.fillAmount = 0;
                isLoad = true;
            }
            else
            {
                loadMask.SetActive(isShow);
                isLoad = false;
            }
        }
    }

    private void SetPosition(Vector3 pos)
    {
        loadMask.transform.position = pos;
    }

    public void Loading(Vector3 pos, ButtonType type)
    {
        ShowOrHide(true);
        SetPosition(pos);
        this.type = type;
    }

    public void SetMaskHierarchy()
    {
        loadMask.transform.SetAsLastSibling();
    }

    private void Update()
    {
        if(isLoad)
        {
            mImage_LoadMask.fillAmount = Mathf.Lerp(mImage_LoadMask.fillAmount, 1, Time.deltaTime * smooth);
            if (mImage_LoadMask.fillAmount >= 0.95f)
            {
                mImage_LoadMask.fillAmount = 1;
                ShowOrHide(false);
                ButtonEvent.OnButtonTriggerEvent(type);
                isLoad = false;
            }
        }
    }
}
