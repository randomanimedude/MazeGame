using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    public Vector2Int size = new Vector2Int(256, 256);

    //public float scale = 20f;

    //public Vector2 offset = new Vector2();

    PerlinNoiseRandom random = new PerlinNoiseRandom();

    private void Start()
    {
        random.InitRandom(size);
    }

    private void Update()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();
    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(size.x, size.y);

        for(int x = 0; x < size.x; ++x)
        {
            for(int y = 0; y < size.y; ++y)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }

    Color CalculateColor(int x, int y)
    {
        //float xPerlin = (float)x / size.x * scale + offset.x;
        //float yPerlin = (float)y / size.y * scale + offset.y; 

        //float perlinData = Mathf.PerlinNoise(xPerlin, yPerlin);

        float perlinData = random.GetRandomFloatAtPos(0, 1, x, y);
        return new Color(perlinData, perlinData, perlinData);
    }
}
