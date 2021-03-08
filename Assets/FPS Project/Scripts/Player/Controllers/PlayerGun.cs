using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    #region GunVariables
    [SerializeField] private float bullet_distance;
    [SerializeField] private int gunDamage = 1;
    [SerializeField] private float fireRate = .25f;
    [SerializeField] private float weaponRange = 50;
    [SerializeField] private float hitForce = 100;
    [SerializeField] private Transform gunEnd;
    [SerializeField] private GameObject muzzleFlash;
    //[SerializeField] private Animator recoil;
    [SerializeField] private PauseMenuHandler pause;

    [SerializeField] private float bulletSpeed;
    [SerializeField] private Bullet projectile;

    [SerializeField] private GameObject bulletPrefab;

    #region Private Variables
    private Camera fpsCam;
    private WaitForSeconds shotDuration = new WaitForSeconds(.07f);
    private AudioSource gunShotSound;
    //private LineRenderer laserLine;
    private float nextFire;
    #endregion
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        muzzleFlash.SetActive(false);
        #region GunStuff
        //laserLine = GetComponent<LineRenderer>();
        gunShotSound = GetComponent<AudioSource>();
        fpsCam = GetComponentInParent<Camera>();
        //recoil = GetComponent<Animator>();
        #endregion 
    }

    // Update is called once per frame
    void Update()
    {
        if (pause.paused == false)
        {
            PlayerShooting();
            ViewRay();
        }
       
    }

    /// <summary>
    /// Handles the logic for shooting when fire is pressed
    /// </summary>
    private void PlayerShooting()
    {
        // if the player presses the fire button (mouse 1) and the time is greater then next fire
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            #region Raycasting and effects
            nextFire = Time.time + fireRate;    // next fire is reset 
            StartCoroutine(ShotEffect());       // start the coroutine for the shooting effects

            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));        // generates a ray from the center of the fps camera
            RaycastHit hit;                                                                     // a variable for getting hit information

            //laserLine.SetPosition(0, gunEnd.position);                                          // sets the starting point of the linecast
            #endregion

            // the raycast hits something within weapon range
            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))     
            {
                GameObject newBullet = Instantiate(bulletPrefab, gunEnd.position, gunEnd.rotation);
                StartCoroutine(MoveForward(newBullet, hit.point));

                #region Old Code

                //laserLine.SetPosition(1, hit.point);            // sets the line to end at the point of contact           

                //ShootableObject health = 
                //hit.collider.GetComponent<ShootableObject>();   // checks for an object with the shootableObject script and assigns it to health

                //Enemy _health = 
                //hit.collider.GetComponent<Enemy>();

                //// if the script is not null and is assigned
                //if (health != null)
                //{
                //    health.Damage(gunDamage);                   // damage the object and or enemy
                //}

                //if (_health != null)
                //{
                //    _health.DamageEnemy(gunDamage * 10);
                //}

                //// if it has a rigidbody
                //if (hit.rigidbody != null)
                //{
                   // hit.rigidbody.AddForce(-hit.normal * hitForce);     // addforce to the object in the direction hit
                //}

                #endregion
            }
            else  // if the raycast doesnt hit anything ie the player shoots the sky then set the linecast to end at weapon range
            {
                //laserLine.SetPosition(1, rayOrigin + (fpsCam.transform.forward * weaponRange));
                GameObject newBullet = Instantiate(bulletPrefab, gunEnd.position, gunEnd.rotation);
                StartCoroutine(MoveForward(newBullet, rayOrigin + (fpsCam.transform.forward * weaponRange)));
            }

            Debug.Log(hit);
        }
    }

    /// <summary>
    /// Handles the debugging of the ray so that it can be viewed when testing
    /// </summary>
    private void ViewRay()
    {
        Vector3 lineOrigin = fpsCam.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 0));     // calculates the origin of the ray drawing it from the centre of the fps cam 
        Debug.DrawRay (lineOrigin, fpsCam.transform.forward * weaponRange, Color.green);    // draws a visualisable line for the tester to view 
                                                                                            // (fps transform forward draws it forward times'd by the weapon range)
    }

    /// <summary>
    /// Handles the weapons effects and sound 
    /// </summary>
    private IEnumerator ShotEffect()
    {
        gunShotSound.Play();            // plays the gunshot sound when called
        muzzleFlash.SetActive(true);
        //laserLine.enabled = true;       // enables the line
        //recoil.Play(0);
        yield return shotDuration;      // returns the shot duration which waits for set seconds
        //laserLine.enabled = false;      // once time is up disable line
        muzzleFlash.SetActive(false);
    }

    private IEnumerator MoveForward(GameObject bullet, Vector3 target)
    {
        while(bullet != null)
        {                       
            bullet.transform.position = Vector3.MoveTowards(bullet.transform.position,target, Time.deltaTime * bulletSpeed);
            if (Vector3.Distance(bullet.transform.position, target) < bullet_distance)
            {
                Debug.Log("Reached End Pos");
                Destroy(bullet.gameObject,.5f);
                projectile = bullet.GetComponent<Bullet>();
                projectile.enabled = true;
                break;
            }
            yield return null;
        }
    }
}
