using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultRandom : RandomNumberProviderBase
{
    public override void InitRandom()
    {
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
    }

    public override int GetRandomInt(int min, int max)
    {
        return UnityEngine.Random.Range(min, max + 1);
    }

    public override float GetRandomFloat(float min, float max)
    {
        //return Mathf.FloorToInt(UnityEngine.Random.Range(min, max));
        return UnityEngine.Random.Range(min, max);
    }
}
