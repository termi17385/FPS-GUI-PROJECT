using UnityEngine;
using UnityEngine.UI;

public class RandomColor : MonoBehaviour
{
    public Image square;
    public bool disco = false;

    private void Update()
    {
        if (disco == true)
        {
            RandomiseColour();
        }
        
    }

    public void RandomiseColour()
    {
        float redCol = Random.Range(0.0f, 1.0f);
        float blueCol = Random.Range(0.0f, 1.0f);
        float greenCol = Random.Range(0.0f, 1.0f);

        float transCol = Random.Range(0.0f, 0.1f);

        Color randomCol = new Color(redCol, blueCol, greenCol, 1);

        square.color = randomCol;
    }

    public void DiscoMode(bool _disco)
    {
        disco = _disco;
    } 
}
