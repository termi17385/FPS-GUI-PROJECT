using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSProject.Weapons
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] protected float bulletDistance;
        [SerializeField] protected int damage;
        [SerializeField] protected float weaponRange = 50f;
        [SerializeField] protected float fireRate = .25f;
        [SerializeField] protected float bulletSpeed = 40f;
        [SerializeField] protected Transform gunEnd;
        [SerializeField] protected GameObject bulletPrefab;
        [SerializeField] protected float nextFire;

        protected PauseMenuHandler p;
        protected GameManager gm;
        protected Camera fpsCam;
        protected WaitForSeconds shotDuration = new WaitForSeconds(.07f);

        protected virtual void Start()
        {
            fpsCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            p = GameObject.FindGameObjectWithTag("Menu").GetComponent<PauseMenuHandler>();
            gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }

        protected virtual void Update()
        {
            //{ if (Input.GetButton("Fire1")) { Shoot(); } ViewRay(); }
        }

        protected virtual void Shoot()
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                StartCoroutine(ShotEffect());

                Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hit;

                if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
                {
                    GameObject newBullet = Instantiate(bulletPrefab, gunEnd.position, gunEnd.rotation);
                    StartCoroutine(MoveForward(newBullet, rayOrigin + (fpsCam.transform.forward * weaponRange)));
                }
                else
                {
                    GameObject newBullet = Instantiate(bulletPrefab, gunEnd.position, gunEnd.rotation);
                    StartCoroutine(MoveForward(newBullet, rayOrigin + (fpsCam.transform.forward * weaponRange)));
                }
                
            }
        }

        protected virtual void ViewRay()
        {
            Vector3 lineOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            Debug.DrawRay(lineOrigin, fpsCam.transform.forward * weaponRange, Color.green);
        }

        protected virtual IEnumerator ShotEffect()
        {
            yield return shotDuration; 
        }

        protected virtual IEnumerator MoveForward(GameObject bullet, Vector3 target)
        {
            while (bullet != null)
            {
                bullet.transform.position = Vector3.MoveTowards(bullet.transform.position, target, Time.deltaTime * bulletSpeed);
                if (Vector3.Distance(bullet.transform.position, target) < bulletDistance)
                {
                    Debug.Log("Reached End Pos");
                    Destroy(bullet.gameObject, .5f);
                    break;
                }
                yield return null;
            }
        }
    }
}    

