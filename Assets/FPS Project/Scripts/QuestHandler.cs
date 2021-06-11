using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Handles assigning quest ids and setting up the quest for the quest manager
 */
public static class QuestHandler
{
    //[SerializeField]
    //private QuestManager qManager;  
        
    /// <summary>
    /// This Quest Type handles quests that involve remaining targets,
    /// items, etc.
    /// </summary>
    /// <param name="_remaining">the amount of remaining items, targets, etc</param>
    public static void QuestSetup(QuestManager questManager, string _name,out int _id)
    {
        var index =0;
        foreach (var quest in questManager.currentQuests)
        {
            if(_name == quest.questName)
            {
                index = quest.id;
                break;
            }
        }
        _id = index;
    }
}
