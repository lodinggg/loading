using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressToStart : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject plane;
    public GameObject startText;

    void Update()
    {
        if(Input.anyKeyDown)
        {
            mainMenu.SetActive(true);
            plane.SetActive(false);
            startText.SetActive(false);
        }
    }
}
