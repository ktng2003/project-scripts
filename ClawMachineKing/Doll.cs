using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Doll : MonoBehaviour
{
    public List<GameObject> doll_CatchCatch = new List<GameObject>();
    public List<GameObject> doll_PushPush = new List<GameObject>();
    public List<GameObject> doll_SpinSpin = new List<GameObject>();

    public SpriteAtlas atlasCatchCatch;
    public SpriteAtlas atlasPushPush;
    public SpriteAtlas atlasSpinSpin;

    private static Doll instance;
    public static Doll Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<Doll>();

                if (obj != null)
                    instance = obj;
                else
                {
                    var newObj = new GameObject().AddComponent<Doll>();

                    instance = newObj;
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<Doll>();

        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }

        atlasCatchCatch = Resources.Load<SpriteAtlas>("SpriteAtlasCatchCatch");
        atlasPushPush = Resources.Load<SpriteAtlas>("SpriteAtlasPushPush");
        atlasSpinSpin = Resources.Load<SpriteAtlas>("SpriteAtlasSpinSpin");
    }
}