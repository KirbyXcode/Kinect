using UnityEngine;
using UnityEngine.UI;

public class CursorProperty : MonoBehaviour 
{
    private Garbage curGarbage;
    private Image cursorImage;

    private void Start()
    {
        cursorImage = transform.GetComponentInChildren<Image>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Defines.Tag_Garbage)
        {
            if(IsEmptySlot())
            {
                PickUpGarbage(collision.transform);
                AudioManager.Instance.PlaySound(Defines.Audio_Grab);
            }
        }

        if(!IsEmptySlot() && collision.tag == Defines.Tag_GarbageBin)
        {
            curGarbage.RecycleGarbage();
            cursorImage.sprite = CursorController.Instance.Sprites_Cursor[1];

            //判断垃圾类型和垃圾桶类型是否吻合
            GarbageBin bin = collision.GetComponent<GarbageBin>();
            if(curGarbage.type == bin.type)
            {
                //吻合，显示正确表情图标
                PhizController.Instance.CreateGood(collision.transform);
                //增加分数
                DataManager.Instance.AddScore(10);
                //播放音效
                AudioManager.Instance.PlaySound(Defines.Audio_Right);
            }
            else
            {
                //不吻合，显示错误表情图标
                PhizController.Instance.CreateBad(collision.transform);
                //播放音效
                AudioManager.Instance.PlaySound(Defines.Audio_Wrong);
            }
            curGarbage = null;
        }
    }

    private bool IsEmptySlot()
    {
        if (transform.childCount == 1)
        {
            return true;
        }
        return false;
    }

    private void PickUpGarbage(Transform trans)
    {
        cursorImage.sprite = CursorController.Instance.Sprites_Cursor[0];
        trans.parent = transform;
        trans.SetAsFirstSibling();
        curGarbage = trans.GetComponent<Garbage>();
        curGarbage.PickOrDrop();
        curGarbage.SetPosition();
    }
}
