using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perlin : MonoBehaviour
{
    public int depth = 20;
    public float width = 255;
    public float height = 255;
    public float scale = 5000;
    public int cols;
    public int rows;
    public int octaves;
    public float persistance;
    public float lacunarity;

    public List<GameObject> cubes = new List<GameObject>();
    public float[,] map;

    private void Start()
    {
        map = new float[cols, rows];
        map = GenerateHeights();

        for (int i = 0; i < cols; ++i)
        {
            for (int j = 0; j < rows; ++j)
            {
                int cubeCount = 0;
                float y = map[i, j] * scale;
                while ((int)y > cubeCount)
                {
                    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    //go.transform.position = new Vector3(i, map[i, j] * scale / 2, j);
                    //go.transform.localScale = new Vector3(1, map[i, j] * scale, 1);
                    go.transform.position = new Vector3(i, cubeCount, j);
                    go.transform.localScale = new Vector3(1, 1, 1);
                    cubes.Add(go);
                    cubeCount++;
                }


            }
        }
    }

    private void Update()
    {

        //for (int i = 0; i < cols; ++i)
        //{
        //    for (int j = 0; j < rows; ++j)
        //    {
        //        cubes[i + j * rows].transform.position = new Vector3(i, map[i, j] * scale / 2, j);
        //        cubes[i + j * rows].transform.localScale = new Vector3(1, map[i, j] * scale, 1);
        //    }
        //}
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[cols, rows];

        for(float i = 0; i < cols / 10; i += 0.1f)
        {
            for (float j = 0; j < rows / 10; j += 0.1f)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int o = 0; o < octaves; ++o)
                {
                    float perlinValue = Mathf.PerlinNoise(i, j);
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }


                heights[(int)(i * 10), (int)(j * 10)] = noiseHeight;
            }
        }

        return heights;
    }
}
