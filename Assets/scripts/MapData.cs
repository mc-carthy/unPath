using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapData : MonoBehaviour
{
    public TextAsset textAsset;
    public Texture2D textureMap;

    private string resourcePath = "Mapdata";
    private int width, height;

    private void Start()
    {
        string levelName = SceneManager.GetActiveScene().name;

        if (textAsset == null)
        {
            textAsset = Resources.Load(resourcePath + "/" + levelName) as TextAsset;
        }

        if (textureMap == null)
        {
            textureMap = Resources.Load(resourcePath + "/" + levelName) as Texture2D;
        }
    }

    public List<string> GetMapFromTextFile(TextAsset asset)
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

        return lines;
    }

    public List<string> GetMapFromTextFile()
    {
        return GetMapFromTextFile(textAsset);
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

    public List<string> GetMapFromTexture(Texture2D texture)
    {
        List<string> lines = new List<string>();

        if (texture != null)
        {
            for (int y = 0; y < texture.height; y++)
            {
                string newLine = "";
                for (int x = 0; x < texture.width; x++)
                {
                    if (texture.GetPixel(x, y) == Color.black)
                    {
                        newLine += "1";
                    }
                    else if (texture.GetPixel(x, y) == Color.white)
                    {
                        newLine += "0";
                    }
                    else
                    {
                        newLine += " ";
                    }
                }
                lines.Add(newLine);
            }
        }

        return lines;
    }

    public List<string> GetMapFromTexture()
    {
        return GetMapFromTexture(textureMap);
    }

    public int[,] MakeMap()
    {
        List<string> lines = new List<string>();

        if (textureMap != null)
        {
            lines = GetMapFromTexture();
        }
        else
        {
            lines = GetMapFromTextFile();
        }

        SetDimensions(lines);

        int[,] map = new int[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (lines[y].Length > x)
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