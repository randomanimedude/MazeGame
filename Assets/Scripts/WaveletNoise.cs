using UnityEngine;

public class WaveletNoise : PerlinNoise
{
    public override RandomNumberProviderBase GetRandomImpl()
    {
        return new WaveletNoiseRandom();
    }
}
