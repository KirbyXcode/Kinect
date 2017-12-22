using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopEngine;
using ARPG.Common;
using UnityEngine.UI;
using EventCenter;

public class GarbageSpawn : MonoSingleton<GarbageSpawn> 
{
    private GameObject[] garbages;
    private Sprite[] mSprites_Recyclable;
    private Sprite[] mSprites_Unrecyclable;
    private Sprite[] mSprites_Kitchen;
    private Sprite[] mSprites_Other;

    private void Start()
    {
        garbages = Factory.Instance.LoadAllGarbage();

        mSprites_Recyclable = Factory.Instance.LoadAllSprites(Defines.Sprite_Recyclable);
        mSprites_Unrecyclable = Factory.Instance.LoadAllSprites(Defines.Sprite_Unrecyclable);
        mSprites_Kitchen = Factory.Instance.LoadAllSprites(Defines.Sprite_Kitchen);
        mSprites_Other = Factory.Instance.LoadAllSprites(Defines.Sprite_Other);
    }

    private IEnumerator ISpawn(Transform parent)
    {
        yield return new WaitForSeconds(1);
        while (!DataManager.Instance.IsTimeOut())
        {
            int index = Random.Range(0, garbages.Length);

            GameObject garbage = null;

            switch (index)
            {
                case 0:
                    garbage = GameObjectPool.Instance.CreateObject(Defines.Pool_Recyclable, garbages[index], Vector3.zero, Quaternion.identity);
                    SetSprite(garbage, mSprites_Recyclable);
                    break;
                case 1:
                    garbage = GameObjectPool.Instance.CreateObject(Defines.Pool_Unrecyclable, garbages[index], Vector3.zero, Quaternion.identity);
                    SetSprite(garbage, mSprites_Unrecyclable);
                    break;
                case 2:
                    garbage = GameObjectPool.Instance.CreateObject(Defines.Pool_Kitchen, garbages[index], Vector3.zero, Quaternion.identity);
                    SetSprite(garbage, mSprites_Kitchen);
                    break;
                case 3:
                    garbage = GameObjectPool.Instance.CreateObject(Defines.Pool_Other, garbages[index], Vector3.zero, Quaternion.identity);
                    SetSprite(garbage, mSprites_Other);
                    break;
            }

            garbage.transform.parent = parent;
            garbage.transform.localPosition = new Vector3(Random.Range(-430, 430), 1090, 0);
            garbage.transform.localEulerAngles = Vector3.zero;
            garbage.transform.localScale = Vector3.one;

            Garbage garbageScr = garbage.GetComponent<Garbage>();
            garbageScr.Init();

            yield return new WaitForSeconds(3);
        }

        HideAllGarbages();
    }

    private void HideAllGarbages()
    {
        HideGarbage(Defines.Pool_Recyclable);
        HideGarbage(Defines.Pool_Unrecyclable);
        HideGarbage(Defines.Pool_Kitchen);
        HideGarbage(Defines.Pool_Other);
    }

    private void HideGarbage(string type)
    {
        List<GameObject> list = GameObjectPool.Instance.FindAllUnusable(type);

        if (list != null)
        {
            foreach (GameObject go in list)
            {
                GameObjectPool.Instance.MyDestory(go);
            }
        }
    }

    private void SetSprite(GameObject go, Sprite[] sprites)
    {
        int index = Random.Range(0, sprites.Length);
        go.GetComponent<Image>().sprite = sprites[index];
    }

    public void Spawn(Transform parent)
    {
        StartCoroutine(ISpawn(parent));
    }

    public void StopSpawn()
    {
        HideAllGarbages();
        StopAllCoroutines();
    }

    public void StopGameSpawn()
    {
        StopAllCoroutines();

        ZeroGravity(Defines.Pool_Recyclable);
        ZeroGravity(Defines.Pool_Unrecyclable);
        ZeroGravity(Defines.Pool_Kitchen);
        ZeroGravity(Defines.Pool_Other);
    }

    public void StartGameSpawn()
    {
        ButtonEvent.OnButtonTriggerEvent(ButtonType.Spawn);

        OneGravity(Defines.Pool_Recyclable);
        OneGravity(Defines.Pool_Unrecyclable);
        OneGravity(Defines.Pool_Kitchen);
        OneGravity(Defines.Pool_Other);
    }

    private void ZeroGravity(string type)
    {
        List<GameObject> list = GameObjectPool.Instance.FindAllUnusable(type);

        if (list != null)
        {
            foreach (GameObject go in list)
            {
                go.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
        }
    }

    private void OneGravity(string type)
    {
        List<GameObject> list = GameObjectPool.Instance.FindAllUnusable(type);

        if (list != null)
        {
            foreach (GameObject go in list)
            {
                go.GetComponent<Rigidbody2D>().gravityScale = 1;
            }
        }
    }
}
