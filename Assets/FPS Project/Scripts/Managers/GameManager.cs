using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private GameObject freeCam;
    
    public bool freeCamMode = false;

    // Start is called before the first frame update
    void Start()
    {
        freeCam = GameObject.FindGameObjectWithTag("FreeCam");
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        freeCamMode = false;
        freeCam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        EnableFreeCam();
    }

    private void EnableFreeCam()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            freeCamMode = !freeCamMode;
        }

        switch (freeCamMode)
        {
            case true:
            freeCam.SetActive(true);
            mainCam.enabled = false;
            break;

            case false:
            freeCam.SetActive(false);
            mainCam.enabled = true;
            break;
        }
    }
}
