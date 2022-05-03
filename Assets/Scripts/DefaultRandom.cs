using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultRandom : RandomNumberProviderBase
{
    void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
    }

    public override int GetRandomInt(int min, int max)
    {
        return Random.Range(min, max);
    }

    public override float GetRandomFloat(float min, float max)
    {
        return Mathf.FloorToInt(Random.Range(min, max));
    }
}
