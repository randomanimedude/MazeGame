using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveletNoiseRandom : DefaultRandom
{
    public Vector2Int size = new Vector2Int(256, 256);

    private float scale = 20f;

    public Vector2 offset = new Vector2();
    
    // https://graphics.pixar.com/library/WaveletNoise/paper.pdf
    static float[] noiseTileData;

    static int Mod(int x, int n)
    {
        int m = x % n;
        return (m < 0) ? m + n : m;
    }

    const int ARAD = 16;

    static void Downsample(float[] from, float[] to, int n, int stride)
    {
        float[] aCoeffs = {
            0.000334f, -0.001528f, 0.000410f, 0.003545f, -0.000938f, -0.008233f, 0.002172f, 0.019120f,
            -0.005040f, -0.044412f, 0.011655f, 0.103311f, -0.025936f, -0.243780f, 0.033979f, 0.655340f,
            0.655340f, 0.033979f, -0.243780f, -0.025936f, 0.103311f, 0.011655f, -0.044412f, -0.005040f,
            0.019120f, 0.002172f, -0.008233f, -0.000938f, 0.003546f, 0.000410f, -0.001528f, 0.000334f
        };

        float[] a = new float[2 * ARAD];
        Array.Copy(aCoeffs, ARAD, a, 0, ARAD);

        for (int i = 0; i < n / 2; i++)
        {
            to[i * stride] = 0;
            for (int k = 2 * i - ARAD; k <= 2 * i + ARAD - 1; k++)
                to[i * stride] += a[k - 2 * i + ARAD] * from[Mod(k, n) * stride];
        }
    }

    static void Upsample(float[] from, float[] to, int n, int stride)
    {
        float[] pCoeffs = { 0.25f, 0.75f, 0.75f, 0.25f };

        float[] p = new float[4];
        Array.Copy(pCoeffs, 2, p, 0, 2);

        for (int i = 0; i < n; i++)
        {
            to[i * stride] = 0;
            for (int k = i / 2; k <= i / 2 + 1; k++)
                to[i * stride] += p[i - 2 * k + 2] * from[Mod(k, n / 2) * stride];
        }
    }

    static float[] GenerateGaussianNoise(int size, float mean, float stdDev)
    {
        float[] noise = new float[size];
        float minVal = float.MaxValue;
        float maxVal = float.MinValue;

        // Generate noise and find min and max values
        for (int i = 0; i < size; i++)
        {
            float u1 = 1.0f - UnityEngine.Random.value; // Ensure u1 is in the range (0, 1] to avoid log(0)
            float u2 = 1.0f - UnityEngine.Random.value;

            float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2); // Box-Muller transform
            noise[i] = mean + stdDev * randStdNormal;

            if (noise[i] < minVal) minVal = noise[i];
            if (noise[i] > maxVal) maxVal = noise[i];
        }

        // Normalize noise to range [-1, 1]
        for (int i = 0; i < size; i++)
        {
            noise[i] = (noise[i] - minVal) / (maxVal - minVal) * 2.0f - 1.0f;
        }

        return noise;
    }

    void GenerateNoiseTile(int n)
    {
        if (n % 2 != 0) n++; // Tile size must be even

        int ix, iy, iz, i, sz = n * n * n;
        float[] temp1 = new float[sz];
        float[] temp2 = new float[sz];
        


        const float mean = 0.0f;
        const float stdDev = 1.0f;


        // Use the generated Gaussian noise array as needed

        // Step 1. Fill the tile with random numbers in the range -1 to 1.
        float[] noise = GenerateGaussianNoise(sz, mean, stdDev);

        // Steps 2 and 3. Downsample and upsample the tile
        for (iy = 0; iy < n; iy++)
            for (iz = 0; iz < n; iz++)
            {
                // Each x row
                i = iy * n + iz * n * n;
                Downsample(noise, temp1, n, 1);
                Upsample(temp1, temp2, n, 1);
            }

        for (ix = 0; ix < n; ix++)
            for (iz = 0; iz < n; iz++)
            {
                // Each y row
                i = ix + iz * n * n;
                Downsample(temp2, temp1, n, n);
                Upsample(temp1, temp2, n, n);
            }

        for (ix = 0; ix < n; ix++)
            for (iy = 0; iy < n; iy++)
            {
                // Each z row
                i = ix + iy * n;
                Downsample(temp2, temp1, n, n * n);
                Upsample(temp1, temp2, n, n * n);
            }

        // Step 4. Subtract out the coarse-scale contribution
        for (i = 0; i < sz; i++)
        {
            noise[i] -= temp2[i];
        }

        // Avoid even/odd variance difference by adding an odd-offset version of noise to itself.
        int offset = n / 2;
        if (offset % 2 == 0) offset++;

        for (i = 0, ix = 0; ix < n; ix++)
            for (iy = 0; iy < n; iy++)
                for (iz = 0; iz < n; iz++)
                    temp1[i++] = noise[Mod(ix + offset, n) + Mod(iy + offset, n) * n + Mod(iz + offset, n) * n * n];

        for (i = 0; i < sz; i++)
        {
            noise[i] += temp1[i];
        }

        Array.Resize(ref noiseTileData, sz);
        noiseTileData = noise;
    }

    public override void InitRandom(Vector2Int size)
    {
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        this.size = size;
        offset.x = UnityEngine.Random.Range(0, size.x * scale);
        offset.y = UnityEngine.Random.Range(0, size.x * scale);
        GenerateNoiseTile(size.x);
    }

    public override int GetRandomIntAtPos(int min, int max, int x, int y)
    {
        var Wevelet = GetRandomFloatAtPos(0, 1, x, y);

        return min + Math.Min((int)Math.Floor(Wevelet * (max - min + 1)), max);
    }
    
    public override float GetRandomFloatAtPos(float min, float max, int x, int y)
    {
        float xWevelet = (float)x / size.x * scale + offset.x;
        float yWevelet = (float)y / size.y * scale + offset.y;

        // Gettings a 2d slice from 3d noise using XY axis as pixel coords
        // depth is Z axis, changing it will allow configuring slice depth
        int depth = 0;
        int index = Math.Clamp(x + y * size.x + depth * size.x * size.y, 0, noiseTileData.Length - 1);
        var wavelet = (noiseTileData[index] + 1.0f) * 0.5f; // turning [-1; 1] into [0;1]

        return Math.Clamp(min + wavelet * (max - min), min, max);
    }
}
