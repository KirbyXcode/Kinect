using UnityEngine;

public class DestroyFruit : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "CutFruit")
        {
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.tag == "StartStrawberry")
        {
            UIManager.instance.SwitchPanel(true);
            GameCore.instance.isAlive = true;
            Invoke("CreateFruits", 2);
        }
    }

    private void CreateFruits()
    {
        GameCore.instance.CreateFruit();
        GameCore.instance.IsHideTrails(false);
    }
}
