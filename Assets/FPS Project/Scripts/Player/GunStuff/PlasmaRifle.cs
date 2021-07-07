using System.Collections.Generic;
using System.Collections;
using FPSProject.Keybinds;
using FPSProject.Menu;
using UnityEngine;

namespace FPSProject.Weapons
{
    public class PlasmaRifle : Gun
    {
        [SerializeField] private AudioSource gunSound;
        [SerializeField] private MeshFilter mesh;
          
        protected override void Start()
        {
            mesh.mesh = null;
            base.Start();
            gunSound = GetComponent<AudioSource>();
        }

        protected override void Update()
        {
            if (PauseMenu.instance.paused == false)
            {
                // if player is paused of in free cam the player can't shoot

                //base.Update(); 
                if (BindingManager.BindingPressed("Fire")) Shoot(); 
                ViewRay();

                // runs the animation to aim down sights when the player holds right mouse
                //if(BindingManager.BindingHeld("Aim")) anim.SetBool("isADS", true);
                //else anim.SetBool("isADS", false);
            }
        }

        protected override IEnumerator ShotEffect()
        {
            gunSound.Play();
            yield return shotDuration;
            bullet.SetPosition(0, Vector3.zero);
            bullet.SetPosition(1, Vector3.zero);
        }
    }
}

