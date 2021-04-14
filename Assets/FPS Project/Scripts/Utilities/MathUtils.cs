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
        /// <param name="objectPosY">the objects transform</param>
        /// <param name="targetPos">the targets transform</param>
        /// <param name="rotationSpeed">the speed we want to rotate the object at</param>
        static public void LookAtTarget
        (Transform objectPosY, Vector3 targetPos, float rotationSpeed)
        {
            #region Handles the horizontal Rotation of the object
            Vector3 lookPos = targetPos - objectPosY.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            objectPosY.rotation = Quaternion.Slerp(objectPosY.rotation, rotation, rotationSpeed * Time.deltaTime);
            #endregion
        }


        /// <summary>
        /// Calculates the targets angle and direction from the origin's <br/>
        /// forward transform 
        /// </summary>
        /// <param name="_origin">the object we want to get the forward direction from</param>
        /// <param name="_target">the object we are getting the angle of</param>
        /// <param name="_angle">the value of the angle</param>
        /// <param name="_dist">distance of the target from the origin</param>
        static public void CalculateTargetAngle
        (Transform _origin, Transform _target, out float _angle, out float _dist)
        {
            Vector3 pointA = _origin.transform.forward;              // origins forward
            Vector3 pointB = _target.position - _origin.position;    // direction from target to origin

            _dist = pointB.magnitude;                               // used to get the distance
            _angle = Vector3.Angle(pointA, pointB);                 // the angle of the target from the origin
        }
    }
}

