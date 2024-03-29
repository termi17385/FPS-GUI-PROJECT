using System.Collections.Generic;
using System.Collections;
using FPSProject.Menu;
using UnityEngine;

namespace FPSProject.Weapons
{
    public class PlasmaRifle : Gun
    {
        [SerializeField] private GameObject flash;
        [SerializeField] private Animator anim;
        [SerializeField] private Animator gunAnim;
        [SerializeField] private AudioSource gunSound;

        [SerializeField]bool test;
          
        protected override void Start()
        {
            base.Start();
            gunSound = GetComponent<AudioSource>();
            gunAnim = GetComponent<Animator>();
        }

        protected override void Update()
        {
            if (PauseMenu.instance.paused == false)
            {
                // if player is paused of in free cam the player can't shoot

                //base.Update(); 
                if (Input.GetButton("Fire1")) Shoot(); 
                ViewRay();

                // runs the animation to aim down sights when the player holds right mouse
                if(Input.GetKey(KeyCode.Mouse1)) anim.SetBool("isADS", true);
                else anim.SetBool("isADS", false);
            }
        }

        protected override IEnumerator ShotEffect()
        {
            flash.SetActive(true);
            gunAnim.enabled = true;
            gunAnim.Play(0);
            gunSound.Play();
            yield return shotDuration;
            flash.SetActive(false);
        
        }
    }
}

