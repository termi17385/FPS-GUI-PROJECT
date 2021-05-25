using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questName = "Settlements in need";
    [TextArea]
    public string description = "theres a settlement that needs our help i'll mark it on the map";
    
    public int minLevel = 1;
    public float xpReward = 50;

}
