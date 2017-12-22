using UnityEngine;
using EventCenter;
using DG.Tweening;

public class UIIntro : BasePanel 
{
    private Transform mNorBtn_Close;
    private Transform mPressBtn_Close;
    private bool islhPressed;
    private bool isrhPressed;

    private CursorController cursorCtrl;

    private void Awake()
    {
        mNorBtn_Close = Global.FindChild(transform, "Normal").transform;
        mPressBtn_Close = Global.FindChild(transform, "Press").transform;

        ButtonEvent.ButtonTriggerEvent += ButtonTriggerEvent;

        cursorCtrl = CursorController.Instance;
    }

    private void OnDestroy()
    {
        ButtonEvent.ButtonTriggerEvent -= ButtonTriggerEvent;
    }

    private void ButtonTriggerEvent(ButtonType type)
    {
        if (type == ButtonType.Close)
        {
            //退出游戏介绍面板
            UIManager.Instance.PopPanel();
            //退出遮罩面板
            UIManager.Instance.PopPanel();
            DataManager.Instance.CountDown();
            ButtonEvent.OnButtonTriggerEvent(ButtonType.Spawn);
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.5f);
    }

    public override void OnExit()
    {
        transform.DOScale(0, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }

    private void Update()
    {
        cursorCtrl.ButtonTrigger(mNorBtn_Close, mPressBtn_Close, cursorCtrl.LhScreenPos, ref islhPressed, ButtonType.Close);
        cursorCtrl.ButtonTrigger(mNorBtn_Close, mPressBtn_Close, cursorCtrl.RhScreenPos, ref isrhPressed, ButtonType.Close);
    }
}
