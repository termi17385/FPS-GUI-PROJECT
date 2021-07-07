using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSProject.Weapons
{
    public class Gun : MonoBehaviour
    {
        #region Variables
        [SerializeField] protected float bulletDistance;
        [SerializeField] protected int damage;
        [SerializeField] protected float weaponRange = 50f;
        [SerializeField] protected float fireRate = .25f;
        [SerializeField] protected float nextFire;
        [SerializeField] LayerMask layerMask;

        [SerializeField] protected LineRenderer bullet;

        protected Camera fpsCam;
        protected WaitForSeconds shotDuration = new WaitForSeconds(.07f);
        #endregion

        protected virtual void Start()=> fpsCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        protected virtual void Update()
        {
            //{ if (Input.GetButton("Fire1")) { Shoot(); } ViewRay(); }
        }
        /// <summary>
        /// Handles what happens when the gun shoots
        /// </summary>
        protected virtual void Shoot()
        {
            if (!(Time.time > nextFire)) return;
            nextFire = Time.time + fireRate;
            StartCoroutine(ShotEffect());

            // sets the origin of the raycast 
            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0, 0, 0));
            RaycastHit hit;
            
            bullet.SetPosition(0, transform.position);
            
            // shoots a ray towards a point at a set distance and checks to see if it hits a collider
            if(Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange, layerMask))
            {
                // damages the targets
                if (hit.collider.CompareTag("Target"))
                {
                    var _enemy = hit.collider.gameObject.GetComponent<Enemy>();
                    var _target = hit.collider.gameObject.GetComponent<target>();

                    if (_enemy) _enemy.Damage(damage);
                    else _target.Damage(damage);
                }
                bullet.SetPosition(1, hit.point); 
                //target = setTarget.position;
                // spawns and fires the bullet
                //SpawnAndFireBullet(bulletPrefab, target, setTarget);
            }

            #region Broken Code
            //// sets the origin of the ray to the middle of the screen
            //Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            //RaycastHit hit;

            //if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange, layerMask))
            //{
            //    if(hit.transform.tag == "Target")
            //    {
            //        // spawns a bullet and sets the target 
            //        Transform target = hit.transform;
            //        GameObject newBullet = Instantiate(bulletPrefab, gunEnd.position, gunEnd.rotation);
            //        // moves the bullet towards the ray origin
            //        StartCoroutine(MoveForward(newBullet, rayOrigin + (fpsCam.transform.forward * weaponRange)));
            //        newBullet.GetComponent<Bullet>().targetLocation = target;
            //        newBullet.GetComponent<Bullet>().damageAmount = damage;
            //    }
            //    else
            //    {
            //        // spawns a bullet and sets the target 
            //        GameObject newBullet = Instantiate(bulletPrefab, gunEnd.position, gunEnd.rotation);
            //        // moves the bullet towards the ray origin
            //        StartCoroutine(MoveForward(newBullet, rayOrigin + (fpsCam.transform.forward * weaponRange)));
            //    }
            //}
            //else
            //{
            //    // spawns a bullet and sets the target 
            //    GameObject newBullet = Instantiate(bulletPrefab, gunEnd.position, gunEnd.rotation);
            //    // moves the bullet towards the ray origin
            //    StartCoroutine(MoveForward(newBullet, rayOrigin + (fpsCam.transform.forward * weaponRange)));
            //}
            #endregion
        }

        /// <summary>
        /// Used to display a ray for debugging purposes
        /// </summary>
        protected virtual void ViewRay()
        {
            Vector3 lineOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0, 0, 0));
            Debug.DrawRay(lineOrigin, fpsCam.transform.forward * weaponRange, Color.green);
        }
       
        /// <summary>
        /// Handles the effects of the shot
        /// </summary>
        protected virtual IEnumerator ShotEffect()
        {
            yield return shotDuration; 
        }
        #region Old Code
        ///// <summary>
        ///// Spawns a bullet and fires it towards the target
        ///// </summary>
        ///// <param name="_prefab">The bullet to spawn</param>
        ///// <param name="_target">The position to move the bullet towards</param>
        ///// <param name="_bulletSetTarget">The bullets target</param>
        //protected virtual void SpawnAndFireBullet(GameObject _prefab, Vector3 _target, Transform _bulletSetTarget)
        //{
        //    // spawns a bullet and sets the gameobject to that bullet
        //    GameObject newBullet = Instantiate(_prefab, gunEnd.position, gunEnd.rotation);
        //    if(_bulletSetTarget != null)    // checks if the bullet has a target
        //    {
        //        // sets the bullets target location and damage amount
        //        newBullet.GetComponent<Bullet>().targetLocation = _bulletSetTarget;
        //        newBullet.GetComponent<Bullet>().damageAmount = damage;
        //    }  
        //    // moves the bullet towards the target
        //    StartCoroutine(MoveForward(newBullet, _target));
        //}
        ///// <summary>
        ///// Handles moving the bullet towards a target position
        ///// </summary>
        //protected virtual IEnumerator MoveForward(GameObject _bullet, Vector3 _target)
        //{
        //    while (_bullet != null)
        //    {
        //        // move bullet towards the middle of the screen
        //        _bullet.transform.position = Vector3.MoveTowards(_bullet.transform.position, 
        //        _target, Time.deltaTime * bulletSpeed);

        //        if (Vector3.Distance(_bullet.transform.position, _target) < bulletDistance)
        //        {
        //            Debug.Log("Reached End Pos");
        //            Destroy(_bullet.gameObject);
        //            break;
        //        }
        //        yield return null;
        //    }
        //}
        #endregion
    }
}    

