using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    #region Variables
    public float health;
    public float maxHeatlh;

    public Image healthbar;
    public TextMeshProUGUI number;
    #endregion

    #region Start and Update
    // Start is called before the first frame update
    void Start()
    {
        health = maxHeatlh;
        SetHealth(health);
        number.text = string.Format("{0}/{1}", health, maxHeatlh);
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region Methods
    /// <summary>
    /// Used to damage the player
    /// </summary>
    /// <param name="dmg">how much to damage the player by</param>
    public void Damage(float dmg)
    {
        health -= dmg;
        SetHealth(health);

        if (health <= 0f)
        {
            health = 0f;
        }

        number.text = string.Format("{0}/{1}", health, maxHeatlh);
    }

    /// <summary>
    /// Used to heal the player
    /// </summary>
    /// <param name="heal">how much to heal the player by</param>
    public void Heal(float heal)
    {
        health += heal;
        SetHealth(health);

        if (health >= maxHeatlh)
        {
            health = maxHeatlh;
        }

        number.text = string.Format("{0}/{1}", health, maxHeatlh);
    }

    public void SetHealth(float _health)
    {
        healthbar.fillAmount = Mathf.Clamp01(_health / maxHeatlh);
    }
    #endregion
}
