using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    private RawImage userImage;
    public Image Cursor { get; set; }
    private CanvasGroup canvasGroup;
    private Transform mButton_NewGame;
    private Transform mButton_Dojo;
    private Transform mButton_Quit;
    private Transform mButton_Apple;
    private Transform mButton_Strawberry;
    private Transform mbutton_Boom;
    public GameObject Panel_Start { get; private set; }
    public GameObject Panel_Game { get; private set; }
    public GameObject overlayObject;
    private Text scoreCountUI;
    private Text bestCountUI;
    private Toggle[] lifeToggles;
    private CanvasGroup gamoverCG;
    public float smooth = 1.0f;
    private bool isHandClose = false;
    public Sprite[] cursorSprites;
    //public float smoothCG = 1.0f;
    //private float targetAlpha = 1;
    //public bool IsShowGameOver { get; set; }
    [HideInInspector]
    public GameObject gameOver;
    public Tween GameOverTween { get; set; }
    private Transform titleTF;

    void Start ()
    {
        Panel_Start = transform.Find("Panel_Start").gameObject;
        Panel_Game = transform.Find("Panel_Game").gameObject;
        userImage = transform.Find("UserImage").GetComponent<RawImage>();
        canvasGroup = userImage.GetComponent<CanvasGroup>();
        Cursor = transform.Find("Cursor").GetComponent<Image>();
        mButton_NewGame = transform.Find("Panel_Start/Button_NewGame/NewGameCircle");
        mButton_Dojo = transform.Find("Panel_Start/Button_Dojo/DojoCircle");
        mButton_Quit = transform.Find("Panel_Start/Button_Quit/QuitCircle");
        mButton_Apple = transform.Find("Panel_Start/Button_Dojo/Apple");
        mButton_Strawberry = transform.Find("Panel_Start/Button_NewGame/Strawberry");
        mbutton_Boom = transform.Find("Panel_Start/Button_Quit/Boom");
        titleTF = transform.Find("Panel_Start/Title");
        scoreCountUI = transform.Find("Panel_Game/Score/ScoreCount").GetComponent<Text>();
        bestCountUI = transform.Find("Panel_Game/BestScore").GetComponent<Text>();
        //gamoverCG = transform.Find("Panel_Game/GameOver").GetComponent<CanvasGroup>();
        gameOver = transform.Find("Panel_Game/GameOver").gameObject;
        
        GetLifeToggles();
        UpdateBestScoreUI();
        //print("Hieght" + KinectManager.Instance.GetDepthImageHeight() + "width" + KinectManager.Instance.GetDepthImageWidth());
        UIAnimation(mButton_Dojo);
        UIAnimation(mButton_NewGame);
        UIAnimation(mButton_Quit);
        Panel_Game.SetActive(false);
        gameOver.SetActive(false);
        ShowTitle();
    }

    private static UIManager m_Instance;
    public static UIManager instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = GameObject.Find("Canvas").GetComponent<UIManager>();
                //m_Instance = new GameObject("Singleton of UIManager", typeof(UIManager)).GetComponent<UIManager>();
            }
            return m_Instance;
        }
    }

    private void GetLifeToggles()
    {
        lifeToggles = new Toggle[3];
        for (int i = 0; i < lifeToggles.Length; i++)
        {
            lifeToggles[i] = transform.Find("Panel_Game/Life").GetChild(i).GetComponent<Toggle>();
        }
    }

    public void UpdateLifeUI(int life)
    {
        switch (life)
        {
            case 3:
                for (int i = 0; i < lifeToggles.Length; i++)
                {
                    lifeToggles[i].isOn = false;
                }
                break;
            case 2:
                lifeToggles[0].isOn = true;
                break;
            case 1:
                lifeToggles[1].isOn = true;
                break;
            case 0:
                lifeToggles[2].isOn = true;
                break;
            default:
                break;
        }
    }

    public void ShowTitle()
    {
        Tween tween = titleTF.DOLocalMove(new Vector2(0,250), 1.2f).From();
        tween.SetEase(Ease.OutBounce);
    }

    /// <summary>
    /// 是否有用户在监测范围内
    /// </summary>
    public bool IsUser()
    {
        return KinectManager.Instance.IsInitialized() && KinectManager.Instance.IsUserDetected();
    }

    /// <summary>
    /// 显示用户深度值影像
    /// </summary>
    private void IsUserShow()
    {
        if(IsUser())
        {
            canvasGroup.alpha = 0.7f;
        }
        else
        {
            canvasGroup.alpha = 0;
        }
    }

    /// <summary>
    /// 设置用户深度值影像
    /// </summary>
    public void SetUserImage()
    {
        if(IsUser())
        {
            long userID = KinectManager.Instance.GetPrimaryUserID();
            userImage.texture = KinectManager.Instance.GetUsersLblTex();
        }
    }

    /// <summary>
    /// UI旋转动画
    /// </summary>
    private void UIAnimation(Transform button)
    {
        Tween tween = button.DORotate(new Vector3(0, 0, -360), 6, RotateMode.LocalAxisAdd);
        tween.SetEase(Ease.Linear);
        tween.SetLoops(-1);
    }
	
    /// <summary>
    /// 坐标转换
    /// </summary>
    private void UIInteraction()
    {
        if (IsUser())
        {
            long userID = KinectManager.Instance.GetPrimaryUserID();
            if(KinectManager.Instance.IsJointTracked(userID,(int)KinectInterop.JointType.HandRight))
            {
                //获取右手坐标
                Vector3 rightHandPos = KinectManager.Instance.GetJointKinectPosition(userID, (int)KinectInterop.JointType.HandRight);
                //Vector3 rightHandScreenPos = Camera.main.WorldToScreenPoint(rightHandPos);
                //把右手坐标转成屏幕坐标
                Vector2 rightHandScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, rightHandPos);
                //转成Local屏幕坐标
                Vector2 handLocPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(Panel_Start.transform as RectTransform, rightHandScreenPos, Camera.main, out handLocPos);
                //把坐标位置赋值给Cursor
                Cursor.transform.localPosition = handLocPos;
                //var cursorRTF = cursor.transform as RectTransform;
                //cursorRTF.anchoredPosition = rightHandLocalScreenPos;
                InteractonWithHand(userID, rightHandScreenPos);
            }
        }
    }

    private void InteractonWithHand(long userID, Vector2 JointScreenPos)
    {
        KinectInterop.HandState handState = KinectManager.Instance.GetRightHandState(userID);
        if (handState == KinectInterop.HandState.Closed)
        {
            isHandClose = true;
            Cursor.sprite = cursorSprites[1];
        }
        else
        {
            isHandClose = false;
            Cursor.sprite = cursorSprites[0];
        }

        if (ContainCursor(mButton_NewGame, JointScreenPos) && isHandClose == true && mButton_Strawberry.gameObject != null) 
        {
            StartCoroutine(IconDown(mButton_Strawberry));
        }
        else if (ContainCursor(mButton_Dojo, JointScreenPos) && isHandClose == true && mButton_Apple.gameObject != null)
        {
            StartCoroutine(IconDown(mButton_Apple));
        }
        else if (ContainCursor(mButton_Quit, JointScreenPos) && isHandClose == true && mbutton_Boom.gameObject != null)
        {
            Application.Quit();
        }
    }

    public bool ContainCursor(Transform button, Vector2 jointPos)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(button as RectTransform, jointPos, Camera.main);
    }

    private IEnumerator IconDown(Transform buttonFruit)
    {
        Cursor.gameObject.SetActive(false);
        var rb = buttonFruit.GetComponent<Rigidbody2D>();
        rb.gravityScale = 10;
        rb.AddForce(new Vector2(0, 300));
        yield return new WaitForSeconds(3.0f);
        rb.gravityScale = 0;
        buttonFruit.localPosition = Vector3.zero;
    }

    /// <summary>
    /// 官方坐标转换
    /// </summary>
    private void PositionCoordinate()
    {
        if (IsUser())
        {
            long userID = KinectManager.Instance.GetPrimaryUserID();
            if (KinectManager.Instance.IsJointTracked(userID, (int)KinectInterop.JointType.HandRight))
            {
                Vector3 handRightPos = KinectManager.Instance.GetJointKinectPosition(userID, (int)KinectInterop.JointType.HandRight);
                if(handRightPos != Vector3.zero)
                {
                    //3D坐标转深度坐标
                    Vector2 depthPos = KinectManager.Instance.MapSpacePointToDepthCoords(handRightPos);
                    ushort depthValue = KinectManager.Instance.GetDepthForPixel((int)depthPos.x, (int)depthPos.y);

                    if(depthValue > 0)
                    {
                        //深度坐标转彩色坐标
                        Vector2 colorPos = KinectManager.Instance.MapDepthPointToColorCoords(depthPos, depthValue);
                        float xNorm = (float)colorPos.x / KinectManager.Instance.GetColorImageWidth();
                        float yNorm = 1.0f - (float)colorPos.y / KinectManager.Instance.GetColorImageHeight();

                        if (overlayObject)
                        {
                            //视口坐标转屏幕坐标
                            //Vector3 screenPos = Camera.main.ViewportToScreenPoint(new Vector3(xNorm, yNorm, 0));
                            //视口坐标转世界坐标
                            Vector3 overlayPos = Camera.main.ViewportToWorldPoint(new Vector3(xNorm, yNorm, 0));
                            overlayObject.transform.position = Vector3.Lerp(overlayObject.transform.position, overlayPos, smooth * Time.deltaTime);
                        }     
                    }
                }
            }
        }
    }

    public void UpdateScoreUI(int scoreCount)
    {
        scoreCountUI.text = scoreCount.ToString();
    }

    public void UpdateGameOverUI()
    {
        gameOver.transform.localScale = new Vector3(0, 0, 0);
    }

    public void UpdateBestScoreUI()
    {
        bestCountUI.text = "Best " + PlayerPrefs.GetInt("BestScore", 0).ToString();
    }

    public void SwitchPanel(bool toGame)
    {
        if (toGame)
        {
            Panel_Start.SetActive(false);
            Panel_Game.SetActive(true);
        }
        else
        {
            Panel_Start.SetActive(true);
            Panel_Game.SetActive(false);
        }
    }

    public void GameOverUI()
    {
        gameOver.SetActive(true);
        GameOverTween = gameOver.transform.DOScale(1, 1.5f);
    }

    //private void HideAndShowCG()
    //{
    //    if (IsShowGameOver == true)
    //    {
    //        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, smoothCG * Time.deltaTime);
    //        if (Mathf.Abs(canvasGroup.alpha - targetAlpha) < 0.1f)
    //        {
    //            canvasGroup.alpha = 1;
    //            targetAlpha = 0;
    //            canvasGroup.blocksRaycasts = true;
    //        }
    //    }
    //    else
    //    {
    //        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, smoothCG * Time.deltaTime);
    //        if (Mathf.Abs(canvasGroup.alpha - targetAlpha) < 0.05f)
    //        {
    //            canvasGroup.alpha = 0;
    //            targetAlpha = 1;
    //            canvasGroup.blocksRaycasts = false;
    //        }
    //    }
    //}

    void Update ()
    {
        SetUserImage();
        IsUserShow();
        UIInteraction();
        //PositionCoordinate();
        //HideAndShowCG();
    }
}
