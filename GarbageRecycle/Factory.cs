using UnityEngine;
using DevelopEngine;

public class Factory : MonoSingleton<Factory>
{
    public const string PrefabsPath = "Prefabs/";
    public const string GarbagePath = PrefabsPath + "Garbage";
    public const string SpritesPath = PrefabsPath + "Sprites/";
    public const string AudioPath = "Audio/";

    public GameObject LoadGameObject(string resName)
    {
        return Resources.Load<GameObject>(PrefabsPath + resName);
    }

    public GameObject InstGameObject(string resName)
    {
        GameObject go = LoadGameObject(resName);
        if (go == null)
        {
            Debug.LogError("无法加载资源，路径:" + resName);
        }
        return GameObject.Instantiate<GameObject>(go);
    }

    public GameObject[] LoadAllGarbage()
    {
        return Resources.LoadAll<GameObject>(GarbagePath);
    }

    public Sprite[] LoadAllSprites(string resName)
    {
        return Resources.LoadAll<Sprite>(SpritesPath + resName);
    }

    public AudioClip LoadSound(string resName)
    {
        return Resources.Load<AudioClip>(AudioPath + resName);
    }
}
