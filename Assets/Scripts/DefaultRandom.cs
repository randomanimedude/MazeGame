using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultRandom : RandomNumberProviderBase
{
    public override void InitRandom(Vector2Int size)
    {
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
    }

    public override int GetRandomIntAtPos(int min, int max, int x, int y)
    {
        return UnityEngine.Random.Range(min, max + 1);
    }

    public override float GetRandomFloatAtPos(float min, float max, int x, int y)
    {
        //return Mathf.FloorToInt(UnityEngine.Random.Range(min, max));
        return UnityEngine.Random.Range(min, max);
    }
}
