using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPSProject.Keybinds;

public class PressAnyKey : MonoBehaviour
{
    [SerializeField] GameObject[] UI = new GameObject[2];
    [SerializeField] Animator anim;


    private void Awake()
    {
        UI[0] = gameObject;
        UI[1] = transform.GetChild(0).gameObject;
        
        anim = UI[0].GetComponent<Animator>();
        anim.enabled = false;
        
        if(PlayerPrefs.GetInt("FirstTime") == 1)
        {
            UI[0].SetActive(false);
            UI[1].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _PressAnyKey();
    }

    private void _PressAnyKey()
    {
        if (BindingUtils.GetAnyPressedKey() != KeyCode.None)
        {
            anim.enabled = true;
            UI[1].SetActive(false);
            StartCoroutine(PressAnyAnimation());
        }
    }

    IEnumerator PressAnyAnimation()
    {
        PlayerPrefs.SetInt("FirstTime", 1);
        yield return new WaitForSeconds(2);
        UI[0].SetActive(false);
    }
}
