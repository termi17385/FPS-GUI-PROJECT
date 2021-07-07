using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionQuest : MonoBehaviour
{
    private int count;
    public int questID;
    [SerializeField] private QuestManager qManager;
    [SerializeField] private List<GameObject> objs = new List<GameObject>();

    private void Start()
    {
        foreach (Transform child in transform)
        {
            count++;
            objs.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }
    }

    public void EnableQuest()
    {
        foreach (var obj in objs)
        {
            obj.SetActive(true);
        }
    }
    public void Collected(int i)
    {
        count -= i;
        if (count <= 0) QuestOver();
    }

    private void QuestOver()
    {
        qManager.CheckQuest(questID);
    }
}
