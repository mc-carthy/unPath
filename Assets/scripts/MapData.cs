using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapData : MonoBehaviour
{
    private int width = 10;
    private int height = 5;

    public TextAsset textAsset;
    public string resourcePath = "Mapdata";

    public List<string> GetTextFromFile(TextAsset asset)
    {
        List<string> lines = new List<string>();

        if (asset != null)
        {
            string textData = asset.text;
            char[] delimiters = { '\n', '\r' };
            lines = textData.Split(delimiters).ToList();
            
            // We reverse the list as we read text in from the top down,
            // but build the map from the bottom up
            lines.Reverse();
        }
        else
        {
            Debug.LogWarning("MAPDATA - GetTextFromFile Error: invalid TextAsset!");
        }

        return lines;
    }

    public List<string> GetTextFromFile()
    {
        if (textAsset == null)
        {
            string levelName = SceneManager.GetActiveScene().name;
            textAsset = Resources.Load(resourcePath + "/" + levelName) as TextAsset;
        }
        return GetTextFromFile(textAsset);
    }

    public void SetDimensions(List<string> textLines)
    {
        height = textLines.Count;

        foreach (string line in textLines)
        {
            if (line.Length > width)
            {
                width = line.Length;
            }
        }
    }

    public int[,] MakeMap()
    {
        List<string> lines = new List<string>();

        lines = GetTextFromFile();
        SetDimensions(lines);

        int[,] map = new int[width, height];
        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if(lines[y].Length > x)
                {
                    map[x, y] = (int)Char.GetNumericValue(lines[y][x]);
                }
                else
                {
                    map[x, y] = 0;
                }
            }
        }

		return map;
    }
}