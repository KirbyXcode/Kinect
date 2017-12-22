using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCenter;
using UnityEngine.UI;

public class UILeave : BasePanel 
{
    private float intervalTime = 6f;
    private float elapsedTime;
    private Text mText_CountDown;
    private int time;

    private void Awake()
    {
        KinectEvent.UserDetectedEvent += UserDetectedEvent;

        mText_CountDown = transform.Find("CountDown").GetComponent<Text>();
    }

    private void OnDestroy()
    {
        KinectEvent.UserDetectedEvent -= UserDetectedEvent;
    }

    private void UserDetectedEvent(bool isDetected)
    {
        if (isDetected && gameObject.activeInHierarchy) 
        {
            StartGame();
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        transform.SetAsLastSibling();

        StopGame();
        StartCoroutine("CountDown");
    }

    public override void OnExit()
    {
        base.OnExit();
        elapsedTime = 0;
        StopCoroutine("CountDown");
    }

    private IEnumerator CountDown()
    {
        time = 5;
        mText_CountDown.text = time.ToString();
        while (time >= 0) 
        {
            yield return new WaitForSeconds(1);
            time -= 1;
            mText_CountDown.text = time.ToString();
            AudioManager.Instance.PlaySound(Defines.Audio_CountDown);
        }
    }

    private void StopGame()
    {
        if(UIManager.Instance.IsGamePanelActive())
        {
            GarbageSpawn.Instance.StopGameSpawn();
            DataManager.Instance.StopCountDown();
            //Time.timeScale = 0;
        }
        CursorController.Instance.ShowOrHideCursors(false);
    }

    private void StartGame()
    {
        if (UIManager.Instance.IsStartPanelActive())
        {
            CursorController.Instance.ShowOrHideRightCursor(true);
        }
        else
        {
            CursorController.Instance.ShowOrHideCursors(true);
        }

        if(UIManager.Instance.IsGamePanelActive())
        {
            GarbageSpawn.Instance.StartGameSpawn();
            DataManager.Instance.CountDown();
            //Time.timeScale = 1;
        }
        else
        {
            CursorController.Instance.SetCursorsSprite();
        }

        UIManager.Instance.PopPanel();
    }

    private void ExitGame()
    {
        if(UIManager.Instance.IsGamePanelActive())
        {
            CursorController.Instance.RecycleGarbage();
            GarbageSpawn.Instance.StopSpawn();
            CursorController.Instance.SetCursorsSprite();
            DataManager.Instance.StopCountDown();
            //Time.timeScale = 1;
        }

        UIManager.Instance.PopAllPanels();
        UIManager.Instance.PushPanel(UIPanelType.Start);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= intervalTime)
        {
            ExitGame();
        }
    }
}
