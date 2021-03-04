using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderState : StateMachineBehaviour
{
    // runs first frame of the update
    public override void OnStateEnter(Animator _animator, AnimatorStateInfo stateInfo, int _layerIndex)
    {
        base.OnStateEnter(_animator, stateInfo, _layerIndex);
        Debug.Log("Hello World");
    }
  
    // runs every frame expect first and last when we are in this state
    public override void OnStateUpdate(Animator _animator, AnimatorStateInfo stateInfo, int _layerIndex)
    {
        base.OnStateUpdate(_animator, stateInfo, _layerIndex);
    }

    // runs last frame of the update
    public override void OnStateExit(Animator _animator, AnimatorStateInfo stateInfo, int _layerIndex)
    {
        base.OnStateExit(_animator, stateInfo, _layerIndex);

        _animator.SetTrigger("Target");    
    }
}
