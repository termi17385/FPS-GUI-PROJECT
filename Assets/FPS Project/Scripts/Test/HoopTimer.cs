using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoopTimer : MonoBehaviour
{
    static public float x;
    static public float y;
    static public bool b;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    
    // Update is called once per frame
    void Update()
    {
        if (b == true)
        {
            Timer();
        }
        highScoreText.text = y.ToString("f2");
    }

    public void Timer()
    {
        x += Time.fixedDeltaTime;
        timerText.text = x.ToString("f2");
    }
}
