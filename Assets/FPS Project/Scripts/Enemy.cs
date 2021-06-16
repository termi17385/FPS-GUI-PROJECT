using System.Collections;
using FPSProject.Menu;
using UnityEngine.AI;
using UnityEngine;
using FPSProject.Player.Manager;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public Transform healthBar;
    public Image healthBarImage;
    public float dampening;
    private NavMeshAgent _agent;
    private PlayerManager pManager;
    private LineRenderer line;
    private float distance;
    private bool canShoot;
    private AudioSource shootAudio;

    private float health = 100;
    private float maxHealth = 100;

    private void Start()
    {
        shootAudio = GetComponent<AudioSource>();
        line = GetComponent<LineRenderer>();
        pManager = FindObjectOfType<PlayerManager>();
        _agent = GetComponent<NavMeshAgent>();
        canShoot = true;
        healthBarImage.fillAmount = Mathf.Clamp01(health / maxHealth);
    } 
    
    private void Update()
    {
        if (!PauseMenu.instance.paused)
        {
            Range();
            LookAtThePlayer();
            if (target != null)
            {
                _agent.SetDestination(target.position);
            }

            if (distance <= 40)
            {
                RaycastHit hit;
                if (Physics.Linecast(transform.position, target.position, out hit))
                {   
                    if (hit.collider.CompareTag("Player")) 
                    {
                        if (canShoot)
                        {
                            StartCoroutine(Attack(10));
                            canShoot = false;
                        }
                    }
                }
            }
        }
    }

    #region methods

    private void HealthBar(int dmg)
    {
        health -= dmg;
        healthBarImage.fillAmount = Mathf.Clamp01(health / maxHealth);
    }    
    private void Range()
    {
        distance = Vector3.Distance(transform.position, target.position);
    }
    
    IEnumerator Attack(int amt)
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, target.position);
        shootAudio.Play();
        pManager.DamagePlayer(amt);
        yield return new WaitForSecondsRealtime(0.3f);
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);
        yield return new WaitForSecondsRealtime(2);
        canShoot = true;
    }
    
    public void Damage(int amt)
    {
            StartCoroutine(HitMarker());
            AudioSource sound = pManager.soundEffect;
            Debug.Log("Player Hit target for" + amt + "damage");
            sound.Play();
            HealthBar(amt);
    }
    
    IEnumerator HitMarker()
    {
        GameObject hitMarker = pManager.hitMarker;
        hitMarker.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        hitMarker.SetActive(false);
        if(health <= 0) gameObject.SetActive(false);
    }
    private void LookAtThePlayer()
    {
        Vector3 lookPos = target.position - healthBar.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        healthBar.rotation = Quaternion.Slerp(healthBar.rotation, rotation, Time.deltaTime * dampening);
    }
    #endregion
}
