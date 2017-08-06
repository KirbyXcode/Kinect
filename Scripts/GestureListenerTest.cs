using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestureListenerTest : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    private Text gestureText;
    private Rigidbody ballRb;
    private GameObject ball;
    private GameObject hand;
    private GameObject kickLeg;
    public int upPower = 250;
    public int downPower = 300;
    private bool isReady = false;

    private void Start()
    {
        gestureText = GameObject.Find("Text_Gesture").GetComponent<Text>();
        gestureText.text = "用户丢失";
        ballRb = GameObject.Find("Ball").GetComponent<Rigidbody>();
        ball = GameObject.Find("Ball");
        hand = GameObject.FindWithTag("BallOnHand");
        kickLeg = GameObject.FindWithTag("KickLeg");
        SetBallOnHand();
        isReady = false;
    }

    private void SetBallOnHand()
    {
        ball.transform.SetParent(hand.transform);
        ball.transform.localPosition = new Vector3(0, 0.17f, 0);
        ballRb.useGravity = false;
    }

    private void GetBallBack()
    {
        if (KinectManager.Instance.IsInitialized() && KinectManager.Instance.IsUserDetected())
        {
            long userID = KinectManager.Instance.GetPrimaryUserID();
            KinectInterop.HandState handState = KinectManager.Instance.GetRightHandState(userID);
            if (handState == KinectInterop.HandState.Closed)
            {
                SetBallOnHand();
            }
        }
    }

    public bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture, KinectInterop.JointType joint)
    {
        return true;
    }

    public bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture, KinectInterop.JointType joint, Vector3 screenPos)
    {
        if(gesture == KinectGestures.Gestures.Tpose)
        {
            SetBallOnHand();
        }
        //if (gesture == KinectGestures.Gestures.SwipeUp && IsTouchBall(hand))
        //{
        //    ball.transform.parent = null;
        //    ballRb.useGravity = true;
        //    ballRb.AddForce(Vector3.up * upPower);
        //}
        else if (gesture == KinectGestures.Gestures.KickRight && IsTouchBall(kickLeg))
        {
            ball.transform.parent = null;
            ballRb.useGravity = true;
            ballRb.AddForce(Vector3.up * upPower);
        }
        else if (gesture == KinectGestures.Gestures.SwipeDown && IsTouchBall(hand))
        {
            isReady = true;
            ball.transform.parent = null;
            ballRb.useGravity = true;
            ballRb.AddForce(Vector3.up * -downPower);
        }
        return true;
    }

    private bool IsTouchBall(GameObject targetGo)
    {
        return Vector3.Distance(ball.transform.position, targetGo.transform.position) < 0.7f;
    }

    public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture, float progress, KinectInterop.JointType joint, Vector3 screenPos)
    {

    }

    public void UserDetected(long userId, int userIndex)
    {
        gestureText.text = "用户已检测到\n摆出Tpose,球返回原位";
    }

    public void UserLost(long userId, int userIndex)
    {
        gestureText.text = "用户丢失";
    }

    private void BallDown()
    {
        if (isReady == true && hand.transform.position.y + 0.2f < ball.transform.position.y)
        {
            ballRb.AddForce(Vector3.up * -200);
        }
    }

    private void Update()
    {
        //GetBallBack();
        BallDown();
    }
}
