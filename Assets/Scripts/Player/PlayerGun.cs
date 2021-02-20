using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    #region GunVariables
    [SerializeField] private int gunDamage = 1;
    [SerializeField] private float fireRate = .25f;
    [SerializeField] private float weaponRange = 50;
    [SerializeField] private float hitForce = 100;
    [SerializeField] private Transform gunEnd;

    #region Private Variables
    private Camera fpsCam;
    private WaitForSeconds shotDuration = new WaitForSeconds(.07f);
    private AudioSource gunShotSound;
    private LineRenderer laserLine;
    private float nextFire;
    #endregion
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        #region GunStuff
        laserLine = GetComponent<LineRenderer>();
        gunShotSound = GetComponent<AudioSource>();
        fpsCam = GetComponentInParent<Camera>();
        #endregion 
    }

    // Update is called once per frame
    void Update()
    {
        PlayerShooting();
        ViewRay();
    }

    /// <summary>
    /// Handles the logic for shooting when fire is pressed
    /// </summary>
    private void PlayerShooting()
    {
        // if the player presses the fire button (mouse 1) and the time is greater then next fire
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;    // next fire is reset 
            StartCoroutine(ShotEffect());       // start the coroutine for the shooting effects

            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));        // generates a ray from the center of the fps camera
            RaycastHit hit;                                                                     // a variable for getting hit information

            laserLine.SetPosition(0, gunEnd.position);                                          // sets the starting point of the linecast

            // the raycast hits something within weapon range
            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))     
            {
                laserLine.SetPosition(1, hit.point);            // sets the line to end at the point of contact           

                ShootableObject health = 
                hit.collider.GetComponent<ShootableObject>();   // checks for an object with the shootableObject script and assigns it to health

                // if the script is not null and is assigned
                if (health != null)
                {
                    health.Damage(gunDamage);                   // damage the object and or enemy
                }

                // if it has a rigidbody
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * hitForce);     // addforce to the object in the direction hit
                }
            }
            else  // if the raycast doesnt hit anything ie the player shoots the sky then set the linecast to end at weapon range
            {
                laserLine.SetPosition(1, rayOrigin + (fpsCam.transform.forward * weaponRange));
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

        laserLine.enabled = true;       // enables the line
        yield return shotDuration;      // returns the shot duration which waits for set seconds
        laserLine.enabled = false;      // once time is up disable line
    }
}
