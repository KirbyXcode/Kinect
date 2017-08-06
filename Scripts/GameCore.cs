using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameCore : MonoBehaviour
{
    public GameObject fruitPrefab;
    public Sprite[] fruitSprites;
    private const int forceY = 6500;
    private const int forceX = 400;
    private const int cutForceX = 800;
    private const float fruitOut = -426;
    private GameObject fruitGo;
    private GameObject[] trails;
    private GameObject[] cutFruits;
    private int ranNum;
    private int scoreCount;
    private int life = 3;
    public bool isAlive = true;

    private static GameCore m_Instance;
    public static GameCore instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = GameObject.Find("Canvas").GetComponent<GameCore>();
            }
            return m_Instance;
        }
    }

    private void Start()
    {
        trails = GameObject.FindGameObjectsWithTag("Trail");
        cutFruits = Resources.LoadAll<GameObject>("CutFruits");
        IsHideTrails(true);
    }

    public void IsHideTrails(bool flag)
    {
        if(flag)
        {
            for (int i = 0; i < trails.Length; i++)
            {
                trails[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < trails.Length; i++)
            {
                trails[i].SetActive(true);
            }
        }
    }

    public void CreateFruit()
    {
        if (isAlive == false) return;
        fruitGo = Instantiate(fruitPrefab) as GameObject;
        fruitGo.transform.SetParent(UIManager.instance.Panel_Game.transform);
        fruitGo.transform.localPosition = new Vector3(Random.Range(-585, 585), -280, 0);
        fruitGo.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        ranNum = Random.Range(0, 6);
        var fruitImg = fruitGo.GetComponent<Image>();
        fruitImg.sprite = fruitSprites[ranNum];
        fruitImg.SetNativeSize();
        if(ranNum != (int)FruitsType.Boom)
        {
            fruitGo.GetComponentInChildren<ParticleSystem>().gameObject.SetActive(false);
        }
        else
        {
            fruitGo.GetComponentInChildren<ParticleSystem>().gameObject.SetActive(true);
        }
        var rb = fruitGo.GetComponent<Rigidbody2D>();
        if (fruitGo.transform.localPosition.x > 0)
        {
            rb.AddForce(new Vector2(-forceX, forceY));
        }
        else
        {
            rb.AddForce(new Vector2(forceX, forceY));
        }
    }

    /// <summary>
    /// 水果超出底边
    /// </summary>
    private void FruitDestroy()
    {
        if (fruitGo != null) 
        {
            if (fruitGo.transform.localPosition.y < fruitOut)
            {
                OutBorderScoreUpdate();
                UIManager.instance.UpdateLifeUI(life);
                IsGameOver();
                Destroy(fruitGo);
                CreateFruit();
            }
        }
    }

    private void OutBorderScoreUpdate()
    {
        if(ranNum != (int)FruitsType.Boom)
        {
            scoreCount--;
            life--;
            UIManager.instance.UpdateScoreUI(scoreCount);
        }
    }

    /// <summary>
    /// 切中水果
    /// </summary>
    private void CutFruit(KinectInterop.JointType joint, GameObject trail)
    {
        if (UIManager.instance.IsUser())
        {
            long userID = KinectManager.Instance.GetPrimaryUserID();
            if (KinectManager.Instance.IsJointTracked(userID, (int)joint))
            {
                //获取关节坐标
                Vector3 jointPos = KinectManager.Instance.GetJointKinectPosition(userID, (int)joint);
                //拖尾效果
                trail.transform.position = jointPos;
                //把关节坐标转成屏幕坐标
                KinectInterop.HandState handState = 0;
                if (joint == KinectInterop.JointType.HandLeft)
                {
                    handState = KinectManager.Instance.GetLeftHandState(userID);
                }
                else if(joint == KinectInterop.JointType.HandRight)
                {
                    handState = KinectManager.Instance.GetRightHandState(userID);
                }
                Vector2 jointSenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, jointPos);
                if (fruitGo == null) return;
                if (UIManager.instance.ContainCursor(fruitGo.transform, jointSenPos) && handState == KinectInterop.HandState.Open) 
                {
                    CutFruitScoreUpdate();
                    UIManager.instance.UpdateLifeUI(life);
                    IsGameOver();
                    Destroy(fruitGo);
                    CreateCutFruits();
                    CreateFruit();
                }
            }
        }
    }

    private void CutFruitScoreUpdate()
    {
        if (ranNum == (int)FruitsType.Boom)
        {
            scoreCount--;
            life--;
            UIManager.instance.UpdateScoreUI(scoreCount);
        }
        else
        {
            scoreCount++;
            UIManager.instance.UpdateScoreUI(scoreCount);
        }
    }

    private void CreateCutFruits()
    {
        switch (ranNum)
        {
            case (int)FruitsType.Apple:
                FruitSplit(cutFruits[(int)CutType.LeftApple], -cutForceX);
                FruitSplit(cutFruits[(int)CutType.RightApple], cutForceX);
                break;
            case (int)FruitsType.Banana:
                FruitSplit(cutFruits[(int)CutType.LeftBanana], -cutForceX);
                FruitSplit(cutFruits[(int)CutType.RightBanana], cutForceX);
                break;
            case (int)FruitsType.Strawberry:
                FruitSplit(cutFruits[(int)CutType.LeftStrawberry], -cutForceX);
                FruitSplit(cutFruits[(int)CutType.RightStrawberry], cutForceX);
                break;
            case (int)FruitsType.Peach:
                FruitSplit(cutFruits[(int)CutType.LeftPeach], -cutForceX);
                FruitSplit(cutFruits[(int)CutType.RightPeach], cutForceX);
                break;
            case (int)FruitsType.Sandia:
                FruitSplit(cutFruits[(int)CutType.LeftSandia], -cutForceX);
                FruitSplit(cutFruits[(int)CutType.RightSandia], cutForceX);
                break;
            default:
                break;
        }
    }

    private void FruitSplit(GameObject go, int cutForceX)
    {
        var cutFruit = Instantiate(go, fruitGo.transform.position, transform.rotation) as GameObject;
        cutFruit.transform.SetParent(UIManager.instance.Panel_Game.transform);
        cutFruit.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        cutFruit.GetComponent<Rigidbody2D>().AddForce(new Vector2(cutForceX, 500));
    }


    private void IsGameOver()
    {
        if (life <= 0) 
        {
            isAlive = false;
            if (PlayerPrefs.GetInt("BestScore", 0) < scoreCount) 
            {
                PlayerPrefs.SetInt("BestScore", scoreCount);
            }
            UIManager.instance.GameOverUI();
            UIManager.instance.GameOverTween.OnComplete(GameReset);
        }
    }
    private void GameReset()
    {
        life = 3;
        UIManager.instance.UpdateLifeUI(life);
        scoreCount = 0;
        UIManager.instance.UpdateScoreUI(scoreCount);
        UIManager.instance.Cursor.gameObject.SetActive(true);
        UIManager.instance.UpdateBestScoreUI();
        UIManager.instance.UpdateGameOverUI();
        IsHideTrails(true);
        UIManager.instance.SwitchPanel(false);
        UIManager.instance.ShowTitle();
    }

	void Update ()
    {
        FruitDestroy();
        CutFruit(KinectInterop.JointType.HandLeft, trails[0]);
        CutFruit(KinectInterop.JointType.HandRight, trails[1]);
    }
}
