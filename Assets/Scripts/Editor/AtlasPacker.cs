using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public static class AtlasPacker
{
    [MenuItem("Assets/PackFolder")]
    public static void PackFolder()
    {
        if(Selection.objects.Length <= 0)
            return;
        
        Object selectedObject = Selection.objects[0];
     
        //it is expected that selected asset is directory
        string dirPath = AssetDatabase.GetAssetPath(selectedObject);

        if(!Directory.Exists(dirPath))
            return;
        
        //collect png files from selected folder
        string[] files = Directory.GetFiles(dirPath, "*.png", SearchOption.TopDirectoryOnly);

        List<Texture> textures = new List<Texture>();

        //load png files as textures
        foreach (string file in files)
        {
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(file);
            
            if(texture != null)
                textures.Add(texture);
        }

        //extract groups (for instance CYBRA1 has texture group A) 
        Dictionary<char, List<Texture2D>> textureGroups = new Dictionary<char, List<Texture2D>>();

        //character name exactly the same as folder name
        string characterName = dirPath.Split("/").Last();
        
        foreach (Texture2D texture in textures)
        {
            string textureName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(texture));
            
            //find character name substring index
            int indexOfCharacterName = textureName.IndexOf(characterName, StringComparison.InvariantCultureIgnoreCase);
            //get next letter after character name
            char textureGroupChar = textureName[indexOfCharacterName + characterName.Length];

            if (!textureGroups.TryGetValue(textureGroupChar, out List<Texture2D> textureGroup))
            {
                textureGroup = new List<Texture2D>();
                textureGroups.Add(textureGroupChar, textureGroup);
            }

            textureGroup.Add(texture);
        }

        int targetWidth = textureGroups.SelectMany(group => group.Value).Max(texture => texture.width);
        int targetHeight = textureGroups.SelectMany(group => group.Value).Max(texture => texture.height);
        
        foreach (var textureGroup in textureGroups)
        {
            Texture2D texture = CreateSpritesheet(textureGroup.Value, targetWidth, targetHeight);
            SaveTexture(texture, dirPath + $"/Packed/{characterName + textureGroup.Key}.png");
        }
    }

    
    public static Texture2D CreateSpritesheet(List<Texture2D> textures, int targetWidth, int targetHeight)
    {   
        const int padding = 4;
        
        int singleSpriteWidth  = targetWidth;
        int singleSpriteHeight = textures.Max(tex => tex.height);

        int rowsCount = (textures.Count - 1) / 4 + 1;
        int columnCount = Mathf.Min(4, textures.Count);

        int height = rowsCount * singleSpriteHeight + (rowsCount + 1) * padding;
        int width = columnCount * targetWidth + (columnCount + 1) * padding;
        
        Texture2D spritesheet = new Texture2D(width, height, TextureFormat.ARGB32, false);

        Color[] pixels = new Color[width * height];
        Array.Fill(pixels, Color.clear);
        spritesheet.SetPixels(0, 0, width, height, pixels);
        
        
        for (int i = 0; i < textures.Count; i++)
        {
            int row = rowsCount - i / 4 - 1;
            int column = i % 4;

            Texture2D targetTexture = textures[i];

            int xOffset = (singleSpriteWidth - targetTexture.width) / 2;
                
            spritesheet.SetPixels(column * (singleSpriteWidth + padding) + padding + xOffset, row * (singleSpriteHeight + padding) + padding, textures[i].width, textures[i].height, textures[i].GetPixels()); 
        }
        
        spritesheet.Apply();
        return spritesheet;
    }

    public static void SaveTexture(Texture2D texture, string path)
    {
        byte[] bytes = texture.EncodeToPNG();

        string directoryName = Path.GetDirectoryName(path);
        
        if(string.IsNullOrEmpty(directoryName))
            return;
        
        if(!Directory.Exists(directoryName))
            Directory.CreateDirectory(directoryName);
        
        File.WriteAllBytes(path, bytes);
        AssetDatabase.ImportAsset(path);
        
        TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(path);
 
        importer.isReadable = true;
        importer.textureType = TextureImporterType.Default;
 
        TextureImporterSettings importerSettings = new TextureImporterSettings();
        importer.ReadTextureSettings(importerSettings);
        importerSettings.spriteExtrude = 0;
        importer.SetTextureSettings(importerSettings);

        importer.npotScale = TextureImporterNPOTScale.ToNearest;
        importer.maxTextureSize = 2048;
        importer.alphaIsTransparency = true;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.alphaSource = TextureImporterAlphaSource.FromInput;
        importer.filterMode = FilterMode.Point;
 
        EditorUtility.SetDirty(importer);
        importer.SaveAndReimport();
        
        AssetDatabase.Refresh();
    }
}
