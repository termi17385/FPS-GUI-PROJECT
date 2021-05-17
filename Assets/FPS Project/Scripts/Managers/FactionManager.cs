using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Factions
{
    float _approval;
    [SerializeField]
    public float approval
    {
        set
        {
            _approval = Mathf.Clamp(value, -1, 1);
        }
        get
        {
            return _approval;
        }
    }
}

public class FactionManager : SerializedMonoBehaviour
{
    [SerializeField]
    Dictionary<string, Factions> factions;

    public static FactionManager instance;

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
        
        factions = new Dictionary<string, Factions>();
        factions.Add("TheSettlementClan", new Factions());
        factions.Add("Empire", new Factions());
    }

    public float? FactionsApproval(string factionName, float value)
    {
        if (factions.ContainsKey(factionName))
        {
            factions[factionName].approval += value;
            return factions[factionName].approval;
        }
        
        return null;
    }

    public float? FactionsApproval(string factionName)
    {
        if (factions.ContainsKey(factionName))
        {
            return factions[factionName].approval;
        }

        return null;
    }
}
