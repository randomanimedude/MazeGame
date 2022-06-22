using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseRandom : DefaultRandom
{
    public Vector2Int size = new Vector2Int(256, 256);

    private float scale = 20f;

    public Vector2 offset = new Vector2();

    public override void InitRandom(Vector2Int size)
    {
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        this.size = size;
        offset.x = UnityEngine.Random.Range(0, size.x * scale);
        offset.y = UnityEngine.Random.Range(0, size.y * scale);
    }

    public override int GetRandomIntAtPos(int min, int max, int x, int y)
    {
        var perlin = GetRandomFloatAtPos(0, 1, x, y);

        return min + Math.Min((int)Math.Floor(perlin * (max - min + 1)), max);
    }
    

    public override float GetRandomFloatAtPos(float min, float max, int x, int y)
    {
        float xPerlin = (float)x / size.x * scale + offset.x;
        float yPerlin = (float)y / size.y * scale + offset.y;

        var perlin = Mathf.PerlinNoise(xPerlin, yPerlin);

        // Prelin must return [0;1] but sometimes returns negative
        return Math.Clamp(min + perlin * (max - min), min, max);
    }
}
