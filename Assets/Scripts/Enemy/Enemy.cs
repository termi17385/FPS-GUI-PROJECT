using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Enemy : MonoBehaviour
{      
    [SerializeField] private Transform player;
    [SerializeField] private Transform head;

    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float health;
    [SerializeField] private Image bar;
    [SerializeField] private float target_range;
    [SerializeField] private float enemySpeed;
    [SerializeField] private float enemyLookSpeed;
    [SerializeField] private float weaponRange;
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private float nextFire;
    [SerializeField] private LineRenderer laserLine;
    [SerializeField] private Transform gunEnd;
    [SerializeField] private GameObject muzzleFlash;

    private WaitForSeconds shotDuration = new WaitForSeconds(.07f);

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

        #region EnemyMovement
        EnemyMove();
        EnemyLookAtPlayer();
        #endregion
        SetHealthEnemy(health);
        #region Enemy Shooting
        ViewRay();
        ShootAtPlayer();
        #endregion
    }

    private void EnemyMove()
    {
        if (Vector3.Distance(transform.position, player.position) > target_range)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, enemySpeed * Time.deltaTime);
        }
    }

    private void EnemyLookAtPlayer()
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

        #region used to rotate the body (left and right)
        // gets the position for the object to rotate towards
        Vector3 lookPos = player.position - transform.position;
        lookPos.y = 0; // locks the rotation to the y axis
        // sets the rotation of the object
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, enemyLookSpeed * Time.deltaTime); // smooths the rotation and handles rotating the object
        #endregion

        #region used for the gun and head (up and down)
        // gets the position for the object to rotate towards
        Vector3 lookPos_ = player.position - head.position;
        lookPos.x = Mathf.Clamp(rotation.x, -15, 15); // locks the rotation to the y axis
        // sets the rotation of the object
        Quaternion rotation_ = Quaternion.LookRotation(lookPos_);
        head.rotation= Quaternion.Slerp(head.rotation, rotation_, enemyLookSpeed * Time.deltaTime); // smooths the rotation and handles rotating the object
        #endregion
    }

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

    private IEnumerator ShotEffect()
    {
        muzzleFlash.SetActive(true);
        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false;
        muzzleFlash.SetActive(false);
    }
}
