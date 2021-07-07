using UnityEngine;

namespace FPSProject.Utils
{
    static class TextureUtils
    {
        public static Texture2D LoadTextureResource(string _resourceName)
        {
            object[] obj = Resources.LoadAll("InventoryItems/Sprites");
            foreach(object texture in obj)
            {
                string textureName = texture.ToString();
                if (textureName.Contains(_resourceName)) return texture as Texture2D;
            }

            return null;
        }
        
        public static GameObject LoadGameObjectResource(string _resourceName)
        {
            object[] obj = Resources.LoadAll("InventoryItems/GameObjects");
            foreach(object mesh in obj)
            {
                string meshName = mesh.ToString();
                if (meshName.Contains(_resourceName)) return mesh as GameObject;
            }

            return null;
        }
        
        public static Mesh LoadMeshResource(string _resourceName)
        {
            object[] obj = Resources.LoadAll("InventoryItems/Meshes");
            foreach(object mesh in obj)
            {
                string meshName = mesh.ToString();
                if (meshName.Contains(_resourceName)) return mesh as Mesh;
            }

            return null;
        }
    }
}