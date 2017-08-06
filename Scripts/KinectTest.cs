using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KinectTest : MonoBehaviour {

    public RawImage tex_RawImage;
    private KinectManager manager;

    void Start ()
    {
    }

    private void GetUserImage()
    {
        manager = KinectManager.Instance;
        if(manager.IsInitialized())
        {
            //Texture2D texColor2D = manager.GetUsersClrTex(); //获得彩色数据流
            Texture2D texDepth2D = manager.GetUsersLblTex();   //获得深度数据流
            tex_RawImage.texture = texDepth2D;
        }
    }

    /// <summary>
    /// 获取骨骼节点位置
    /// </summary>
    private void GetJointPos()
    {
        //判断是否有获取到用户ID
        if (manager.IsUserDetected())
        {
            //获取用户ID
            long userID = manager.GetPrimaryUserID();
            //获取骨骼节点类型
            int jointType = (int)KinectInterop.JointType.HandLeft;
            //判断是否骨骼节点有捕获到
            if (manager.IsJointTracked(userID, jointType))
            {
                //获取骨骼节点位置
                Vector3 leftHandPos = manager.GetJointPosition(userID, jointType); //Y轴坐标加上Sensor Height后的值
                Vector3 leftHandKinectPos = manager.GetJointKinectPosition(userID, jointType); //Y轴坐标没有加上Sensor Height后的值
            }
        }
    }
	
    /// <summary>
    /// 获取手势姿势
    /// </summary>
    private void GetHandGesture()
    {
        //判断是否有获取到用户ID
        if (manager.IsUserDetected())
        {
            //获取用户ID
            long userID = manager.GetPrimaryUserID();
            //获取骨骼节点类型
            int jointType = (int)KinectInterop.JointType.HandRight;
            //判断是否骨骼节点有捕获到
            if (manager.IsJointTracked(userID, jointType))
            {
                KinectInterop.HandState handState = manager.GetRightHandState(userID);
                switch (handState)
                {
                    case KinectInterop.HandState.Open:
                        print("右手展开");
                        break;
                    case KinectInterop.HandState.Closed:
                        print("右手握拳");
                        break;
                    case KinectInterop.HandState.Lasso:
                        print("yes 手势");
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 获取用户位置
    /// </summary>
    private void GetUserPos()
    {
        if(manager.IsUserDetected())
        {
            long userID = manager.GetPrimaryUserID();
            Vector3 userPos = manager.GetUserPosition(userID);
        }
    }

	void Update ()
    {

    }
}
