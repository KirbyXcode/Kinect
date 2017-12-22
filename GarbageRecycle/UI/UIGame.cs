using UnityEngine;
using UnityEngine.UI;
using EventCenter;

public class UIGame : BasePanel 
{
    private Text mText_Time;
    private Text mText_Score;

    private GameObject user;
    private RawImage userImage;

    private void Awake()
    {
        mText_Time = Global.FindChild<Text>(transform, "Time");
        mText_Score = Global.FindChild<Text>(transform, "Score");

        user = transform.Find("User").gameObject;
        userImage = user.GetComponent<RawImage>();

        DataEvent.DataTriggerEvent += DataTriggerEvent;
        ButtonEvent.ButtonTriggerEvent += ButtonTriggerEvent;
    }
  
    private void OnDestroy()
    {
        DataEvent.DataTriggerEvent -= DataTriggerEvent;
        ButtonEvent.ButtonTriggerEvent -= ButtonTriggerEvent;
    }

    private void ButtonTriggerEvent(ButtonType type)
    {
        switch (type)
        {
            case ButtonType.Over:
                UIManager.Instance.PushPanel(UIPanelType.Mask);
                UIManager.Instance.PushPanel(UIPanelType.Over);
                CursorController.Instance.SetCursorsHierarchy();
                CursorController.Instance.SetCursorsSprite();
                LoadController.Instance.SetMaskHierarchy();
                break;
            case ButtonType.Spawn:
                GarbageSpawn.Instance.Spawn(this.transform);
                break;
        }
    }

    private void DataTriggerEvent(DataType type, int data)
    {
        switch (type)
        {
            case DataType.Score:
                SetScore(data);
                break;
            case DataType.Time:
                SetTime(data);
                break;
        }
    }

    private void SetScore(int value)
    {
        mText_Score.text = value.ToString();
    }

    private void SetTime(int value)
    {
        mText_Time.text = Global.GetMinuteTime(value);
    }

    private void SetUserMap(bool isShow)
    {
        if (isShow)
        {
            user.SetActive(isShow);
            userImage.texture = KinectManager.Instance.GetUsersLblTex();
        }
        else
        {
            user.SetActive(isShow);
            userImage.texture = null;
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        SetUserMap(true);
        
    }

    public override void OnExit()
    {
        base.OnExit();
        SetUserMap(false);
    }
}
