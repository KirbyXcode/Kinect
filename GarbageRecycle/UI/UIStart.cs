using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCenter;

public class UIStart : BasePanel 
{
    private Transform mNorBtn_Start;
    private Transform mPressBtn_Start;
    private CursorController cursorCtrl;
    private bool isPressStart;

    private void Awake()
    {
        cursorCtrl = CursorController.Instance;
        mNorBtn_Start = Global.FindChild(transform, "StartNormal").transform;
        mPressBtn_Start = Global.FindChild(transform, "StartPress").transform;

        ButtonEvent.ButtonTriggerEvent += ButtonTriggerEvent;
    }

    private void OnDestroy()
    {
        ButtonEvent.ButtonTriggerEvent -= ButtonTriggerEvent;
    }

    private void ButtonTriggerEvent(ButtonType type)
    {
        if(type == ButtonType.Start)
        {
            UIManager.Instance.PopPanel();
            UIManager.Instance.PushPanel(UIPanelType.Game);
            UIManager.Instance.PushPanel(UIPanelType.Mask);
            UIManager.Instance.PushPanel(UIPanelType.Intro);
            cursorCtrl.ShowOrHideCursors(true);
            cursorCtrl.SetCursorsSprite();
            cursorCtrl.SetCursorsHierarchy();
            LoadController.Instance.SetMaskHierarchy();
            DataManager.Instance.InitData();
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private void Update()
    {
        cursorCtrl.ButtonTrigger(mNorBtn_Start, mPressBtn_Start, cursorCtrl.RhScreenPos, ref isPressStart, ButtonType.Start);
    }
}
