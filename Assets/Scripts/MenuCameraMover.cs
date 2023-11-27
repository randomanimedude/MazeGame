using System.Collections;
using UnityEngine;

public class MenuCameraMover : MonoBehaviour
{
    public MazeRenderer maze;
    public float speed = 1.0f;

    private Vector3[] corners;
    private int currentCorner = 0;
    private bool waitingForMazeInit = true;

    public float distanceThreshold = 1.0f;
    public float cornerOffset = 1.0f;

    public Transform[] points;
    private float t = 0;

    private void Init()
    {
        Transform[] walls = maze.GetComponentsInChildren<Transform>(false);
        if (walls.Length == 0)
        {
            return;
        }

        corners = new Vector3[4];

        float minX = Mathf.Infinity, minY = Mathf.Infinity, maxX = -Mathf.Infinity, maxY = -Mathf.Infinity;
        foreach (Transform wall in walls)
        {
            Vector3 position = wall.position;
            minX = Mathf.Min(minX, position.x);
            minY = Mathf.Min(minY, position.y);
            maxX = Mathf.Max(maxX, position.x);
            maxY = Mathf.Max(maxY, position.y);
        }

        minX += cornerOffset;
        minY += cornerOffset;
        maxX -= cornerOffset;
        maxY -= cornerOffset;

        corners[0] = new Vector3(minX, minY, -12);
        corners[1] = new Vector3(maxX, minY, -12);
        corners[2] = new Vector3(maxX, maxY, -12);
        corners[3] = new Vector3(minX, maxY, -12);

        transform.position = corners[currentCorner];

        points = new Transform[4];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = new GameObject("Point " + i).transform;
            points[i].position = new Vector3(
                Random.Range(corners[0].x, corners[1].x),
                Random.Range(corners[0].y, corners[2].y),
                Random.Range(-20, -24)
            );
        }

        waitingForMazeInit = false;
    }

    void Update()
    {
        if (waitingForMazeInit)
        {
            Init();
            if(waitingForMazeInit)
            {
                return;
            }
        }

        transform.position = CatmullRom(points[0].position, points[1].position, points[2].position, points[3].position, t);
        t += Time.deltaTime * speed;
        if (t > 1)
        {
            t = 0;
            Transform temp = points[0];
            for (int i = 0; i < points.Length - 1; i++)
            {
                points[i] = points[i + 1];
            }
            points[points.Length - 1] = temp;
            Vector3 center = (corners[0] + corners[2]) / 2;
            if (Vector3.Distance(transform.position, center) > distanceThreshold)
            {
                points[points.Length - 1].position = center + new Vector3(
                    Random.Range(-cornerOffset, cornerOffset),
                    Random.Range(-cornerOffset, cornerOffset),
                    Random.Range(-12, -6)
                );
            }
            else
            {
                points[points.Length - 1].position = new Vector3(
                    Random.Range(corners[0].x, corners[1].x),
                    Random.Range(corners[0].y, corners[2].y),
                    Random.Range(-12, -6)
                );
            }
        }
    }
    public static Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 a = 2f * p1;
        Vector3 b = p2 - p0;
        Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
        Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

        return 0.5f * (a + b * t + c * t * t + d * t * t * t);
    }
}