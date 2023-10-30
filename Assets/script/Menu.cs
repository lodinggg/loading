
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject startText;
    public GameObject gameTitle;
    public Text customPageTitle;
    public GameObject customPage;
    public GameObject editPage;
    public GameObject savingPanel;

    public GameObject mainCam;
    public GameObject customCam;
    public GameObject plane;
    public GameObject player;

    private bool IsGameStarted;
    private bool isEdit;


    private void Start()
    {
        IsGameStarted = false;
        isEdit = false;
    }
    void Update()
    {
        if(!IsGameStarted)
        {
            if (Input.anyKeyDown)
            {
                mainMenu.SetActive(true);
                plane.SetActive(false);
                startText.SetActive(false);
                IsGameStarted = true;
                mainCam.transform.SetParent(player.transform, true);

            }
        }

        
    }

    //메인메뉴 페이지 상호작용
    public void clickedCustomBtn()
    {
        gameTitle.SetActive(false);
        mainMenu.SetActive(false);
        customPage.SetActive(true);
        customPageTitle.gameObject.SetActive(true);

        plane.SetActive(true);
        player.transform.position = new Vector3(3, 0, -5);
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
        mainCam.SetActive(false);
        customCam.SetActive(true);
    }

    public void clicekedExitBtn()
    {
        Application.Quit();
    }
    //커스텀 페이지 메뉴 상호작용

    public void clickedCustomEditBtn()
    {
        setEdit(true);
    }
    public void clickedBackBtn()
    {
        if (isEdit)
        {
            savingPanel.SetActive(true);

        }
        else
        {
            gameTitle.SetActive(true);
            mainMenu.SetActive(true);
            customPage.SetActive(false);
            customPageTitle.gameObject.SetActive(false);
            plane.SetActive(false);
            player.transform.position = new Vector3(3, 0, -5);
            player.transform.rotation = Quaternion.Euler(-90, 0, 150);
            mainCam.SetActive(true);
            customCam.SetActive(false);
        }
    }
    private void setEdit(bool _isEdit)
    {
        isEdit = _isEdit;
    }
    public void clickedCustomYes()
    {
        setEdit(false);
        savingPanel.SetActive(false);
        clickedBackBtn();
    }

    public void clickedCustomNo()
    {
        setEdit(false);
        savingPanel.SetActive(false);
        clickedBackBtn();
    }

    public void readyToEdit()
    {
        customPage.SetActive(false);
        editPage.SetActive(true);
    }
}
