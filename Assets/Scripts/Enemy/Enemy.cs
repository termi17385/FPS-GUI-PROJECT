using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Enemy : MonoBehaviour
{      
    [SerializeField] private Transform player;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float health;
    [SerializeField] private Image bar;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
        transform.position += transform.forward * 2f * Time.deltaTime;
        SetHealthEnemy(health);
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
}
