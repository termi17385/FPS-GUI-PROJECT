using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using FPSProject.Player.Manager;
using TMPro;

public class QuestManager : SerializedMonoBehaviour
{
    public List<Quest> currentQuests = new List<Quest>();
    public static QuestManager instance;
    [SerializeField] private PlayerManager pManager;
    [SerializeField] private List<TextMeshProUGUI> text = new List<TextMeshProUGUI>();
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform questHolder;
    [SerializeField] private List<GameObject> oldPrefabs = new List<GameObject>();
    
    void Start()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);

        pManager = FindObjectOfType<PlayerManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ViewQuest();
        }

        if (Input.GetKeyDown(KeyCode.F)) CheckQuest(0);
    }

    public void ViewQuest()
    {
        DestroyOldPrefabs();
        foreach(Quest quest in currentQuests)
        {
            var newQuest = Instantiate(prefab, questHolder);
            oldPrefabs.Add(newQuest);

            string _questName = $"{quest.questName}";
            string _description = $"{quest.description}";
            string _reward = $"+{quest.xpReward}xp";

            TextMeshProUGUI nametext = newQuest.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI descriptiontext = newQuest.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI rewardText = newQuest.transform.Find("Reward").GetComponent<TextMeshProUGUI>();
            
            nametext.text = _questName;
            descriptiontext.text = _description;
            rewardText.text = _reward;
            
            text.Add(nametext);
            text.Add(descriptiontext);
            text.Add(rewardText);
        }
    }

    private void DestroyOldPrefabs()
    {
        if (oldPrefabs.Count > 0)
        {
            for (var index = 0; index < oldPrefabs.Count; index++)
            {
                var prefab = oldPrefabs[index];
                oldPrefabs.RemoveAt(index);
                index--;
                Destroy(prefab);
            }
        }
    }

    public void AcceptQuest(Quest quest)
    {
        currentQuests.Add(quest);
        ViewQuest();
    }

    public void CheckQuest(int index)
    {
        // loops through currentQuests and checks if the quest id matches the index value
        foreach (Quest quest in currentQuests.Where(quest => index == quest.id))
        {
            // assigns the quest then breaks
            CompleteQuest(quest);
            break;
        }
    }
    
    private void CompleteQuest(Quest quest)
    {
        pManager.xpLevel += quest.xpReward;
        currentQuests.Remove(quest); 
        ViewQuest();
    }
}
