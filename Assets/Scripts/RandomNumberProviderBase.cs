using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RandomNumberProviderBase
{
    public abstract void InitRandom();
    public abstract int GetRandomInt(int min, int max);
    public abstract float GetRandomFloat(float min, float max);
}
