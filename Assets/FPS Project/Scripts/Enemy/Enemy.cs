using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using FPSProject.Stats;
using FPSProject.Utils;

public class Enemy : MonoBehaviour
{
    #region Variables
    #region Movement and Targetting Variables
    [Header("Movement and Targetting")]
    [SerializeField] private float enemySpeed;
    [SerializeField] private float enemyLookSpeed;
    [SerializeField] private float target_range;
    #endregion

    #region Health Variables
    [Header("Health")]
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float health;
    #endregion

    #region Weapon Variables
    [Header("Weapons")]
    [SerializeField] private float weaponRange;
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private float nextFire;
    #endregion

    #region Transforms
    [Header("Transforms")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform head;
    [SerializeField] private Transform gun;
    [SerializeField] private Transform gunEnd;
    #endregion

    #region Others
    [Header("Others")]
    [SerializeField] private Animator recoil;
    [SerializeField] private Image bar;
    [SerializeField] private LineRenderer laserLine;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private AudioSource gunShot;
    [SerializeField] private MathUtils utils;
    private WaitForSeconds shotDuration = new WaitForSeconds(.07f);
    #endregion
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        #region old code
        //transform.LookAt(player);
        //transform.position += transform.forward * 2f * Time.deltaTime;
        #endregion

        #region Called Methods
        #region EnemyMovement
        EnemyMove();
        EnemyLookAtPlayer();
        #endregion
        SetHealthEnemy(health);
        #region Enemy Shooting
        ViewRay();
        ShootAtPlayer();
        #endregion
        #endregion
    }

    #region EnemyMovement
    private void EnemyMove()
    {
        if (Vector3.Distance(transform.position, player.position) > target_range)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, enemySpeed * Time.deltaTime);
        }
    }

    private void EnemyLookAtPlayer()   // shorten all of this and add a lot of paras 
    {
        #region old code
        //// get the direction we want to rotate towards
        //Vector3 targetDir = player.position - transform.position;

        //// sets how fast the enemy moves to look at the target
        //float move = enemyLookSpeed * Time.deltaTime;

        //// rotate towards the defined vector towards the target direction
        //Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, move, 0.0f);

        //Debug.DrawRay(transform.position, newDir, Color.red);

        //// calculate a rotation to the target and applies it to this object
        //transform.rotation = Quaternion.LookRotation(newDir);
        // old code doesnt work
        #endregion


        utils.CustomLookAt(transform, player.position, enemyLookSpeed);  // rotates the enemy to stay in line with the player position

        #region used for the gun and head (up and down)
        Vector3 lookPos = player.position - head.position;              //  gets the position of the target
        lookPos.x = Mathf.Clamp(lookPos.x, -15, 15);                    //  sets the axis to rotate the object

        Quaternion rotation_ = Quaternion.LookRotation(lookPos);        //  handles the look rotation
        head.rotation= Quaternion.Slerp(head.rotation, rotation_,       //  rotates the objects with the given values
            enemyLookSpeed * Time.deltaTime);


        Vector3 gunLookPos = player.position - gun.position;            //  gets the position of the target
        gunLookPos.x = Mathf.Clamp(gunLookPos.x, -15, 15);              //  sets the axis to rotate the object

        Quaternion gunRotation = Quaternion.LookRotation(gunLookPos);   //  handles the look rotation
        gun.rotation = Quaternion.Slerp(gun.rotation, gunRotation,      //  rotates the objects with the given values
            enemyLookSpeed * Time.deltaTime);
        #endregion
    }
    #endregion

    #region Shooting
    private void ShootAtPlayer()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            Vector3 rayOrigin = head.position;
            Vector3 rayDir = head.forward;
            RaycastHit hit;

            laserLine.SetPosition(0, gunEnd.position);

            if (Physics.Raycast(rayOrigin, rayDir, out hit, weaponRange))
            {
                PlayerStats player = 
                hit.collider.GetComponent<PlayerStats>();

                if (player != null)
                {
                    Debug.Log("Hit Player");
                    player.Damage(5);
                    laserLine.SetPosition(1, hit.point);
                    StartCoroutine(ShotEffect());
                }
            }
        }
    }

    private void ViewRay()
    {
        Vector3 rayOrigin = head.position;
        Vector3 rayDir = head.forward;

        Debug.DrawRay(rayOrigin, rayDir * weaponRange, Color.red);
    }
    #endregion

    #region Enemy Health
    private void SetHealthEnemy(float health)
    {
        bar.fillAmount = Mathf.Clamp01(health/maxHealth);
    }

    public void DamageEnemy(int dmg)
    {
        health -= dmg;
        Debug.Log("hit");

        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    #endregion

    private IEnumerator ShotEffect()
    {
        muzzleFlash.SetActive(true);
        gunShot.Play();
        laserLine.enabled = true;   
        recoil.Play(0);
        yield return shotDuration;
        laserLine.enabled = false;
        muzzleFlash.SetActive(false);
    }
}
