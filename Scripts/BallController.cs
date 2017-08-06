using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{

    private RawImage userImage;
    private Image cursor;
    private CanvasGroup canvasGroup;
    private Transform Button_NewGame;
    public GameObject overlayObject;
    public float smooth = 1.0f;
    public float distanceToCamera = 100;
    public Camera UICamera;

    void Start()
    {
        userImage = transform.Find("UserImage").GetComponent<RawImage>();
        canvasGroup = userImage.GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// 是否有用户在监测范围内
    /// </summary>
    private bool IsUser()
    {
        return KinectManager.Instance.IsInitialized() && KinectManager.Instance.IsUserDetected();
    }

    /// <summary>
    /// 显示用户深度值影像
    /// </summary>
    private void IsUserShow()
    {
        if (IsUser())
        {
            canvasGroup.alpha = 0;
        }
        else
        {
            canvasGroup.alpha = 0;
        }
    }

    /// <summary>
    /// 设置用户深度值影像
    /// </summary>
    private void SetUserImage()
    {
        if (IsUser())
        {
            long userID = KinectManager.Instance.GetPrimaryUserID();
            userImage.texture = KinectManager.Instance.GetUsersLblTex();
        }
    }

    /// <summary>
    /// 坐标转换
    /// </summary>
    private void UIInteraction()
    {
        if (IsUser())
        {
            long userID = KinectManager.Instance.GetPrimaryUserID();
            if (KinectManager.Instance.IsJointTracked(userID, (int)KinectInterop.JointType.HandRight))
            {
                //获取右手坐标
                Vector3 rightHandPos = KinectManager.Instance.GetJointKinectPosition(userID, (int)KinectInterop.JointType.HandRight);
                //Vector3 rightHandScreenPos = Camera.main.WorldToScreenPoint(rightHandPos);
                //把右手坐标转成屏幕坐标
                Vector2 rightHandScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, rightHandPos);
                //转成Local屏幕坐标
                Vector2 rightHandLocalScreenPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, rightHandScreenPos, null, out rightHandLocalScreenPos);
                //把坐标位置赋值给Cursor
                cursor.transform.localPosition = rightHandLocalScreenPos;
                //var cursorRTF = cursor.transform as RectTransform;
                //cursorRTF.anchoredPosition = rightHandLocalScreenPos;
                if (RectTransformUtility.RectangleContainsScreenPoint(Button_NewGame as RectTransform, rightHandScreenPos, null))
                {
                    KinectInterop.HandState handState = KinectManager.Instance.GetRightHandState(userID);
                    if (handState == KinectInterop.HandState.Closed)
                    {
                        Debug.Log("进入游戏");
                    }
                }
            }
        }
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
                if (handRightPos != Vector3.zero)
                {
                    //3D坐标转深度坐标
                    Vector2 depthPos = KinectManager.Instance.MapSpacePointToDepthCoords(handRightPos);
                    ushort depthValue = KinectManager.Instance.GetDepthForPixel((int)depthPos.x, (int)depthPos.y);

                    if (depthValue > 0)
                    {
                        //深度坐标转彩色坐标
                        Vector2 colorPos = KinectManager.Instance.MapDepthPointToColorCoords(depthPos, depthValue);
                        float xNorm = (float)colorPos.x / KinectManager.Instance.GetColorImageWidth();
                        float yNorm = 1.0f - (float)colorPos.y / KinectManager.Instance.GetColorImageHeight();

                        if (overlayObject)
                        {
                            Vector3 overlayPos = UICamera.ViewportToWorldPoint(new Vector3(xNorm, yNorm, distanceToCamera));
                            overlayObject.transform.position = Vector3.Lerp(overlayObject.transform.position, overlayPos, smooth * Time.deltaTime);
                        }
                    }

                }
            }
        }
    }

    void Update()
    {
        SetUserImage();
        IsUserShow();
        //UIInteraction();
        //PositionCoordinate();
    }
}
