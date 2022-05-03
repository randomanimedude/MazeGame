using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RandomNumberProviderBase : MonoBehaviour
{
    public abstract int GetRandomInt(int min, int max);
    public abstract float GetRandomFloat(float min, float max);
}
