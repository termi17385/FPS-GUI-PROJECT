using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class QuestManager : SerializedMonoBehaviour
{
    public List<Quest> currentQuests = new List<Quest>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ViewQuest();
        }
    }

    public void ViewQuest()
    {
        foreach(Quest quest in currentQuests)
        {
            string questText = string.Format("QuestName: {0}\nDescription: {1}\nReward: {2}",
            quest.questName, quest.description, quest.xpReward);
            
            Debug.Log(questText);
        }
    }

    public void AcceptQuest(Quest quest)
    {
        currentQuests.Add(quest);
    }

    public void CompleteQuest(Quest quest)
    {
        currentQuests.Remove(quest);
    }
}
