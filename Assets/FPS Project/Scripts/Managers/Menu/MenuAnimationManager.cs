using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using Sirenix.OdinInspector;

/* Changelog and error log
 * 
 * animations for when the submenu is closed
 * 
 * a way to close the submenu when play is pressed 
 * again or anywhere on the screen is pressed
 */

namespace FPSProject.Menu.Animations
{
    public class MenuAnimationManager : SerializedMonoBehaviour
    {
        #region Variables
        public static MenuAnimationManager menuAnimMan;
        [SerializeField] private Animator anim;
    
        [SerializeField] private Image[] image;
        [SerializeField] private GameObject[] subMenu;
        [SerializeField] private Transform[] menuPositions;

        [SerializeField] private int index;
        private float openCount;
        private float closeCount;
        #endregion

        void Start() 
        { 
            if(menuAnimMan == null)menuAnimMan = this;
            else Destroy(this);

            index = 0;
        }
        
        public void DisplaySubMenu()
        {
            //index = 0;
            StartCoroutine(SubMenuAnimationOpen());
        }
        public void CloseSubMenu()
        {
            StartCoroutine(SubMenuAnimationClose());
        }

        /// <summary>
        /// Loops through the menus and <br/>
        /// makes sure the lines are at 0.
        /// </summary>
        public void ResetSubMenuAnimation()
        {
            for (int i = 0; i < image.Length; i++)
            {
                image[i].fillAmount = 0;
                subMenu[i].SetActive(false);
            }
        }

        /// <summary>
        /// Runs the animation for displaying the submenu <br/>
        /// when the coroutine is run (play pressed)
        /// </summary>
        private IEnumerator SubMenuAnimationOpen()
        {
            openCount = 0;
            anim.SetBool("Move", true);
            yield return new WaitForSeconds(0.5f);
            while (true)
            {
                //x += (100 * Time.deltaTime);
                //yield return new WaitForSecondsRealtime(0.0000000000001f);
                yield return new WaitForSecondsRealtime(0.02f);
                openCount += 5.5f;
                image[index].fillAmount = Mathf.Clamp01(openCount / 100);
                if (openCount >= 100)
                {
                    subMenu[index].SetActive(true);
                    if (index >= 2)
                    {
                        //index = -1;
                        //for (int i = 0; i < image.Length; i++)
                        //{
                            //image[i].fillAmount = 0;
                            //subMenu[i].SetActive(false);
                        //}
                        break;
                    }
                    index++;
                    openCount = 0;
                }
            }
        }
        /// <summary>
        /// Closes the subMenu
        /// </summary>
        private IEnumerator SubMenuAnimationClose()
        {
            closeCount = 100;
            while (true)
            {
                //x += (100 * Time.deltaTime);
                //yield return new WaitForSecondsRealtime(0.0000000000001f);
                yield return new WaitForSecondsRealtime(0.02f);
                closeCount -= 5.5f;
                image[index].fillAmount = Mathf.Clamp01(closeCount / 100);  // "retracts" the lines
                if (closeCount <= 0)
                {
                    subMenu[index].SetActive(false);                        // deactivates the buttons
                    if (index <= 0)
                    {
                        break;
                    }                                     // stops the loop when the index is at 0 
                    index--;
                    closeCount = 100;
                }
            }
            anim.SetBool("Move", false);                                   // moves the menu back
        }

    }
}
