using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
   [SerializeField] private CollectionQuest cQ;
   [SerializeField] private Transform player;

   private void Update()
   {
      if (!(Vector3.Distance(transform.position, player.position) < 10)) return;
      if (!Input.GetKeyDown(KeyCode.E)) return;
      cQ.Collected(1);
            
      var obj = this.gameObject;
      Destroy(obj);
   }
}
