using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RandomNumberProviderBase
{
    public abstract void InitRandom(Vector2Int size);
    public abstract int GetRandomIntAtPos(int min, int max, int x, int y);
    public abstract float GetRandomFloatAtPos(float min, float max, int x, int y);
}
