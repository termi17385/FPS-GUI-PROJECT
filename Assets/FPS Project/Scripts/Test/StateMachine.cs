using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;

public enum State
{
    Wander,
    Target,
    Attack,
    Damage,
    Die,
    MAX_VALUE
}

public class StateMachine : MonoBehaviour
{
    private Dictionary<State, string> stateCoroutines = new Dictionary<State, string>();

    private Coroutine currentCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        FillDictionary();

        SwapState(State.Wander);
    }

    private void FillDictionary()
    {
        #region Old Code
        //// converts state to int
        //int maxValue = (int)State.MAX_VALUE;

        //for (int i = 0; i < maxValue; i++)
        //{
        //    // converts int to state
        //    State state = (State)i;

        //    string functionName = state.ToString() + "State";

        //    stateCoroutines.Add(state, functionName);

        //    Debug.Log(state.ToString() + ", " + functionName);

        //}

        #region Understand Linq kinda
        /*
        List<GameObject> objs = new List<GameObject>();
        // goes through the list of objects and gets all the names adding them to a list of names
        List<string> names = objs.Select(x => x.name).ToList();
        names.ForEach(x => Debug.Log(x));
        // searches through the list for objects with tag andrew
        // then replaces all objects with the objects tagged andrew
        objs = objs.Where(x => x.tag == "Andrew").ToList();
        */
        #endregion
        #endregion

        // does the same as below but using linq
        //(Enum.GetValues(typeof(State)) as State[]).ToList().ForEach(x => stateCoroutines.Add(x, $"{x}State"));

        // getting all the states of type state in the enum
        foreach (State state in System.Enum.GetValues(typeof(State)))
        {
            // then adds all the states to the coroutine and the name of the state
            stateCoroutines.Add(state, $"{state}State");
        }
    }

    

    public void SwapState(State _newState)
    {
        // Is there a coroutine currently running?
        if (currentCoroutine != null)
        {
            // There is, so we need to stop it
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        // Attempts to start the passed coroutine and cache it so that we know which one is running
        currentCoroutine = StartCoroutine(stateCoroutines[_newState]);
    }

    private IEnumerator WanderState()
    {
        Debug.Log("I am at the start of the Coroutine!");

        yield return new WaitForSeconds(3);

        Debug.Log("I am after the 3 second delay of the Coroutine!");
    }

    private IEnumerator TargetState()
    {
        yield return new WaitForSeconds(3);
    }

    private IEnumerator AttackState()
    {
        yield return new WaitForSeconds(3);
    }

    private IEnumerator DamageState()
    {
        yield return new WaitForSeconds(3);
    }

    private IEnumerator DieState()
    {
        yield return new WaitForSeconds(3);
    }
}