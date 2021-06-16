using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.Serialization;

public class TargetRange : SerializedMonoBehaviour
{
    [SerializeField] private List<target> listOfTargets = new List<target>();
    [SerializeField] private TextMeshProUGUI text;
    [FormerlySerializedAs("amountOfTargets")] [SerializeField, ReadOnly] private int remainingTargets;
    public int index;
    private bool activated = false;
    #region Quest Scripts
    [SerializeField] private QuestManager qManager;
    #endregion

    #region Start and Awake
    private void Awake()
    { 
        remainingTargets = listOfTargets.Count;
        qManager = FindObjectOfType<QuestManager>();
    }
    private void Start() => TargetsHandler(0);
    #endregion

    private void Update()
    {
        if(activated && remainingTargets <= 0) qManager.CheckQuest(index);
    } 
    
    /// <summary>
    /// Calls the coroutine to reset the targets
    /// </summary>
    public void ResetTargets() => StartCoroutine(TargetsReset());
    public void TargetsHandler(int _amt)
    {
        remainingTargets -= _amt;
        text.text = $"Targets Left: {remainingTargets}";
    }
    
    /// <summary>
    /// Handles Resetting targets
    /// </summary>
    IEnumerator TargetsReset()
    {
        foreach (target _target in listOfTargets)
        {
            StartCoroutine(_target.ResetTargets());
            yield return new WaitForSeconds(0.2f);
        }
        activated = true;
    }
}