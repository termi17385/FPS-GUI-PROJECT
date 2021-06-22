using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Textures", menuName = "Customisation")]
public class Textures : SerializedScriptableObject
{
    public List<Texture2D> textureList = new List<Texture2D>();
    public int index;

    [SerializeField] private new string name;

    private void OnEnable()
    {
        if(textureList.Count == 0) GetTextures(name);
    }

    private void GetTextures(string nameOfTexture)
    {
        object[] obj = Resources.LoadAll("Character");
        foreach(object texture in obj)
        {
            string textureName = texture.ToString();
            if (textureName.Contains(nameOfTexture)) textureList.Add(texture as Texture2D);
        }
    }
}
