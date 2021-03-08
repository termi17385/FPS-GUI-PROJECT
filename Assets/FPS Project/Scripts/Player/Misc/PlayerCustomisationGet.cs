using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomisationGet : MonoBehaviour
{
    public Renderer characterRenderer;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        characterRenderer = GameObject.FindGameObjectWithTag("Mesh").GetComponent<SkinnedMeshRenderer>();
        Load();
    }

    private void Load()
    {
        SetTexture("Skin", PlayerPrefs.GetInt("SkinIndex"));
        SetTexture("Eyes", PlayerPrefs.GetInt("EyesIndex"));
        SetTexture("Mouth", PlayerPrefs.GetInt("MouthIndex"));
        SetTexture("Hair", PlayerPrefs.GetInt("HairIndex"));
        SetTexture("Clothes", PlayerPrefs.GetInt("ClothesIndex"));
        SetTexture("Armour", PlayerPrefs.GetInt("ArmourIndex"));

        player.name = PlayerPrefs.GetString("CharacterName");
    }

    private void SetTexture(string type, int index)
    {
        Texture2D texture = null;
        int matIndex = 0;

        switch (type)
        {
            case "Skin":
            texture = Resources.Load("Character/Skin_" + index) as Texture2D;
            matIndex = 1;
            break;

            case "Eyes":
            texture = Resources.Load("Character/Eyes_" + index) as Texture2D;
            matIndex = 2;
            break;

            case "Mouth":
            texture = Resources.Load("Character/Mouth_" + index) as Texture2D;
            matIndex = 3;
            break;

            case "Hair":
            texture = Resources.Load("Character/Hair_" + index) as Texture2D;
            matIndex = 4;
            break;

            case "Clothes":
            texture = Resources.Load("Character/Clothes_" + index) as Texture2D;
            matIndex = 5;
            break;

            case "Armour":
            texture = Resources.Load("Character/Armour_" + index) as Texture2D;
            matIndex = 6;
            break;
        }

        Material[] mat = characterRenderer.materials;
        mat[matIndex].mainTexture = texture;
        characterRenderer.materials = mat;
    }
}
