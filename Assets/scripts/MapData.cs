using UnityEngine;

public class MapData : MonoBehaviour
{
    public int width = 5;
    public int height = 10;

    public int[,] MakeMap()
    {
        int[,] map = new int[width, height];
        
        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < height; x++)
            {
                map[x, y] = 0;
            }
        }

        map [1, 0] = 1;
        map [1, 1] = 1;
        map [1, 2] = 1;
        map [3, 2] = 1;
        map [3, 3] = 1;
        map [3, 4] = 1;
        map [4, 2] = 1;
        map [5, 1] = 1;
        map [5, 2] = 1;
        map [6, 2] = 1;
        map [6, 3] = 1;
        map [8, 0] = 1;
        map [8, 1] = 1;
        map [8, 2] = 1;
        map [8, 4] = 1;
        
        return map;
    }
}