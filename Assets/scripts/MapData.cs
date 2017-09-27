using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapData : MonoBehaviour
{
    public TextAsset textAsset;
    public Texture2D textureMap;

    public Color32 openColour = Color.white;
    public Color32 blockedColour = Color.black;
    public Color32 lightColour = new Color32(124, 194, 78, 255);
    public Color32 mediumColour = new Color32(252, 255, 52, 255);
    public Color32 heavyColour = new Color32(255, 129, 12, 255);

    private string resourcePath = "Mapdata";
    private int width, height;

    private static Dictionary<Color32, NodeType> terrainLookup = new Dictionary<Color32, NodeType>();

    private void Awake()
    {
        SetupDictionary();
    }

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
                    Color pixelColour = texture.GetPixel(x, y);

                    if (terrainLookup.ContainsKey(pixelColour))
                    {
                        NodeType nodeType = terrainLookup[pixelColour];
                        int nodeTypeNum = (int)nodeType;
                        newLine += nodeTypeNum;
                    }
                    else
                    {
                        newLine += "0";
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

    private void SetupDictionary()
    {
        terrainLookup.Add(openColour, NodeType.Open);
        terrainLookup.Add(blockedColour, NodeType.Blocked);
        terrainLookup.Add(lightColour, NodeType.LightTerrain);
        terrainLookup.Add(mediumColour, NodeType.MediumTerrain);
        terrainLookup.Add(heavyColour, NodeType.HeavyTerrain);
    }

    public static Color GetColourFromNodeType(NodeType nodeType)
    {
        if (terrainLookup.ContainsValue(nodeType))
        {
            return terrainLookup.FirstOrDefault(x => x.Value == nodeType).Key;
        }

        return Color.white;
    }
}