using UnityEngine;
using EventCenter;

public class GameRoot : MonoBehaviour
{
    private KinectManager km;
    private bool isMultiUser;
    private long id;

    private void Awake()
    {
        Screen.SetResolution(607, 1080, true);

        //KinectEvent.UserDetectedEvent += UserDetectedEvent;
    }

    //private void OnDestroy()
    //{
    //    KinectEvent.UserDetectedEvent -= UserDetectedEvent;
    //}

    //private void UserDetectedEvent(bool isDetected)
    //{
    //    if(isDetected)
    //    {
    //        id = km.GetPrimaryUserID();
    //    }
    //    else
    //    {
    //        id = 0;
    //    }
    //}

    void Start()
    {
        km = KinectManager.Instance;
        UIManager.Instance.PushPanel(UIPanelType.Start);
        CursorController.Instance.InitCursors();
        LoadController.Instance.InitLoadMask();
    }

    private void Update()
    {
        if (!km.IsInitialized()) return;

        if (km.IsUserDetected()) 
        {
            if (!Global.isUser)
            {
                id = km.GetPrimaryUserID();

                if (UIManager.Instance.IsStartPanelActive())
                {
                    CursorController.Instance.ShowOrHideRightCursor(true);
                    CursorController.Instance.SetCursorsSprite();
                }
                KinectEvent.OnUserDetectedEvent(true);
                Global.isUser = true;
            }
        }
        else
        {
            if (Global.isUser)  
            {
                UIManager.Instance.PushPanel(UIPanelType.Leave);
                Global.isUser = false;
            }
        }

        if(km.GetUsersCount() > 1)
        {
            if(!isMultiUser)
            {
                if(id != km.GetPrimaryUserID())
                {
                    UIManager.Instance.PushPanel(UIPanelType.Leave);
                }
                isMultiUser = true;
            }
        }
        else
        {
            if(isMultiUser)
            {
                isMultiUser = false;
            }
        }
    }

    void OnApplicationQuit()
    {
        UIManager.Instance.ResetData();
    }
}
