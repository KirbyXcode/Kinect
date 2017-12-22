using UnityEngine;
using DevelopEngine;
using UnityEngine.UI;

public class CursorController : MonoSingleton<CursorController> 
{
    private GameObject lhCursor;
    private GameObject rhCursor;
    private Vector3 lhScreenPos;
    private Vector3 rhScreenPos;
    public Vector3 LhScreenPos { get { return lhScreenPos; } }
    public Vector3 RhScreenPos { get { return rhScreenPos; } }
    private KinectManager km;

    private GameObject canvas;
    public Sprite[] Sprites_Cursor { get; private set; }

    private void Start()
    {
        km = KinectManager.Instance;
    }

    private void HideDefaultCursor()
    {
        Cursor.visible = false;
    }

    private void FindCanvas()
    {
        canvas = GameObject.FindGameObjectWithTag(Defines.Tag_Canvas);
    }

    private void InitCursor(ref GameObject cursor)
    {
        cursor = Factory.Instance.InstGameObject(Defines.Cursor);

        cursor.transform.parent = canvas.transform;
        cursor.transform.localPosition = Vector3.zero;
        cursor.transform.localEulerAngles = Vector3.zero;
        cursor.transform.localScale = Vector3.one;
        cursor.SetActive(false);
    }

    private void InitCursorSprites()
    {
        Sprites_Cursor = Factory.Instance.LoadAllSprites(Defines.Sprite_Cursor);
    }

    public void InitCursors()
    {
        HideDefaultCursor();
        FindCanvas();
        InitCursor(ref lhCursor);
        InitCursor(ref rhCursor);
        InitCursorSprites();
    }

    public void SetCursorsHierarchy()
    {
        lhCursor.transform.SetAsLastSibling();
        rhCursor.transform.SetAsLastSibling();
    }

    public void SetCursorsSprite()
    {
        lhCursor.GetComponentInChildren<Image>().sprite = Sprites_Cursor[1];
        rhCursor.GetComponentInChildren<Image>().sprite = Sprites_Cursor[1];
    }

    public void ShowOrHideCursors(bool isShow)
    {
        lhCursor.SetActive(isShow);
        rhCursor.SetActive(isShow);
    }

    public void ShowOrHideRightCursor(bool isShow)
    {
        rhCursor.SetActive(isShow);
    }

    public void RecycleGarbage()
    {
        if(lhCursor.transform.childCount == 2)
        {
            lhCursor.GetComponentInChildren<Garbage>().RecycleGarbage();
        }

        if(rhCursor.transform.childCount == 2)
        {
            rhCursor.GetComponentInChildren<Garbage>().RecycleGarbage();
        }
    }

    //public void AddPhysicalSystem()
    //{
    //    BoxCollider2D lhCollider = lhCursor.AddComponent<BoxCollider2D>();
    //    lhCollider.isTrigger = true;
    //    Rigidbody2D lhRb = lhCursor.AddComponent<Rigidbody2D>();
    //    lhRb.isKinematic = true;

    //    BoxCollider2D rhCollider = rhCursor.AddComponent<BoxCollider2D>();
    //    rhCollider.isTrigger = true;
    //    Rigidbody2D rhRb = rhCursor.AddComponent<Rigidbody2D>();
    //    rhRb.isKinematic = true;
    //}

    private void BindCursor(KinectInterop.JointType jointType, GameObject cursor, ref Vector3 screenPos)
    {
        long userID = km.GetPrimaryUserID();
        if (km.IsJointTracked(userID, (int)jointType))
        {
            Vector3 jointPos = km.GetJointKinectPosition(userID, (int)jointType);
            if (jointPos != Vector3.zero)
            {
                //3D坐标转深度坐标
                Vector2 depthPos = km.MapSpacePointToDepthCoords(jointPos);
                ushort depthValue = km.GetDepthForPixel((int)depthPos.x, (int)depthPos.y);

                if (depthValue > 0)
                {
                    //深度坐标转彩色坐标
                    Vector2 colorPos = km.MapDepthPointToColorCoords(depthPos, depthValue);
                    float xNorm = (float)colorPos.x / km.GetColorImageWidth();
                    float yNorm = 1.0f - (float)colorPos.y / km.GetColorImageHeight();

                    //视口坐标转屏幕坐标
                    screenPos = Camera.main.ViewportToScreenPoint(new Vector3(xNorm, yNorm, 0));
                    if (canvas != null)
                    {
                        Vector2 localPos;
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, Camera.main, out localPos);

                        cursor.transform.localPosition = localPos;
                    }
                }
            }
        }
    }

    public void ButtonTrigger(Transform normalBtn, Transform pressBtn, Vector2 cursorScreenPos, ref bool isPressed, ButtonType type)
    {
        if (lhCursor == null || rhCursor == null) return;

        bool isClicked = RectTransformUtility.RectangleContainsScreenPoint(normalBtn as RectTransform, cursorScreenPos, Camera.main);

        if (isClicked)
        {
            if (!isPressed)
            {
                isPressed = true;
                pressBtn.gameObject.SetActive(true);
                LoadController.Instance.Loading(normalBtn.position, type);
            }
        }
        else
        {
            if (isPressed)
            {
                isPressed = false;
                pressBtn.gameObject.SetActive(false);
                LoadController.Instance.ShowOrHide(false);
            }
        }
    }

    private void Update()
    {
        if (Global.isUser)
        {
            BindCursor(KinectInterop.JointType.HandLeft, lhCursor, ref lhScreenPos);
            BindCursor(KinectInterop.JointType.HandRight, rhCursor, ref rhScreenPos);
        }
    }
}
