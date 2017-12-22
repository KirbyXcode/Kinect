using UnityEngine;
using DG.Tweening;
using EventCenter;
using UnityEngine.UI;

public class UIOver : BasePanel 
{
    private CursorController cursorCtrl;

    private Transform mNorBtn_Restart;
    private Transform mPressBtn_Restart;
    private Transform mNorBtn_Back;
    private Transform mPressBtn_Back;
    private bool islhRestart;
    private bool isrhRestart;
    private bool islhBack;
    private bool isrhBack;

    private Transform starBar;
    private Text mText_Score;
    private int score;

    private void Awake()
    {
        ButtonEvent.ButtonTriggerEvent += ButtonTriggerEvent;

        cursorCtrl = CursorController.Instance;
        

        mText_Score = transform.Find("Score").GetComponent<Text>();
        starBar = transform.Find("StarBar");
    }

    private void OnDestroy()
    {
        ButtonEvent.ButtonTriggerEvent -= ButtonTriggerEvent;
    }

    private void Start()
    {
        mNorBtn_Restart = Global.FindChild(transform, "RestartNormal").transform;
        mPressBtn_Restart = Global.FindChild(transform, "RestartPress").transform;
        mNorBtn_Back = Global.FindChild(transform, "BackNormal").transform;
        mPressBtn_Back = Global.FindChild(transform, "BackPress").transform;
    }

    private void ButtonTriggerEvent(ButtonType type)
    {
        switch (type)
        {
            case ButtonType.Restart:
                OnGameRestart();
                break;
            case ButtonType.Back:
                OnGameBack();
                break;
        }
    }

    private void OnGameRestart()
    {
        //退出结算面板
        UIManager.Instance.PopPanel();
        //退出遮罩面板
        UIManager.Instance.PopPanel();
        ButtonEvent.OnButtonTriggerEvent(ButtonType.Spawn);
        DataManager.Instance.InitData();
        DataManager.Instance.CountDown();
        cursorCtrl.ShowOrHideCursors(true);
    }

    private void OnGameBack()
    {
        //退出结算面板
        UIManager.Instance.PopPanel();
        //退出遮罩面板
        UIManager.Instance.PopPanel();
        //退出游戏面板
        UIManager.Instance.PopPanel();
        UIManager.Instance.PushPanel(UIPanelType.Start);
    }

    private void SetScore(int value)
    {
        mText_Score.text = value.ToString();
    }

    private void SetStar(int count)
    {
        for (int i = 0; i < starBar.childCount; i++)
        {
            starBar.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < count; i++)
        {
            starBar.GetChild(i).gameObject.SetActive(true);
        }
    }

    private void StarRating()
    {
        if (score <= 50)
        {
            SetStar(1);
        }
        else if (score <= 100)
        {
            SetStar(2);
        }
        else
        {
            SetStar(3);
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();

        score = DataManager.Instance.Score;

        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.5f);

        StarRating();
        SetScore(score);
    }

    public override void OnExit()
    {
        transform.DOScale(0, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }

    private void Update()
    {
        cursorCtrl.ButtonTrigger(mNorBtn_Restart, mPressBtn_Restart, cursorCtrl.LhScreenPos, ref islhRestart, ButtonType.Restart);
        cursorCtrl.ButtonTrigger(mNorBtn_Restart, mPressBtn_Restart, cursorCtrl.RhScreenPos, ref isrhRestart, ButtonType.Restart);

        cursorCtrl.ButtonTrigger(mNorBtn_Back, mPressBtn_Back, cursorCtrl.LhScreenPos, ref islhBack, ButtonType.Back);
        cursorCtrl.ButtonTrigger(mNorBtn_Back, mPressBtn_Back, cursorCtrl.RhScreenPos, ref isrhBack, ButtonType.Back);
    }
}
