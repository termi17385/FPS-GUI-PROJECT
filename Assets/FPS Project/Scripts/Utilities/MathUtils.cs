using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSProject.Utils
{
    public class MathUtils : MonoBehaviour
    {
        /// <summary>
        /// rotates the object left or right towards a target
        /// </summary>
        /// <param name="objectPos">the objects transform</param>
        /// <param name="targetPos">the targets transform</param>
        /// <param name="rotationSpeed">the speed we want to rotate the object at</param>
        public void CustomLookAt(Transform objectPosY, Vector3 targetPos, float rotationSpeed)
        {
            #region Handles the horizontal Rotation of the object
            Vector3 lookPos = targetPos - objectPosY.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            objectPosY.rotation = Quaternion.Slerp(objectPosY.rotation, rotation, rotationSpeed * Time.deltaTime);
            #endregion
        }
    }
}

