using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    
        [SerializeField] private Image[] image;
        [SerializeField] private GameObject[] subMenu;
        [SerializeField] private Transform[] menuPositions;

        [SerializeField] private int index;
        private float x;
        #endregion

        //[SerializeField] private bool subMenAnim = false;

        // Start is called before the first frame update
        void Start() 
        { 
            if(menuAnimMan == null)menuAnimMan = this;
             else Destroy(this);

            index = 0;
        }

        private void Update()
        {
            Transform pos1 = menuPositions[0];
            Transform pos2 = menuPositions[1];
            Transform menuPos = menuPositions[2];

            Vector2 distToPos1 = Vector2.Distance(menuPos, pos1);
            Vector2 distToPos2 = Vector2.Distance(menuPos, pos2);
        }

        public void DisplaySubMenu()
        {
            //index = 0;
            StartCoroutine(SubMenuAnimationOpen());
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

        private IEnumerator MoveMainMenu()
        {
              yield return null;
        }

        /// <summary>
        /// Runs the animation for displaying the submenu <br/>
        /// when the coroutine is run (play pressed)
        /// </summary>
        private IEnumerator SubMenuAnimationOpen()
        {
            while (true)
            {
                //x += (100 * Time.deltaTime);
                //yield return new WaitForSecondsRealtime(0.0000000000001f);
                yield return new WaitForSeconds(-1);
                x += 5.5f;
                image[index].fillAmount = Mathf.Clamp01(x / 100);
                if (x >= 100)
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
                    x = 0;
                }
            }
        }
    }
}
