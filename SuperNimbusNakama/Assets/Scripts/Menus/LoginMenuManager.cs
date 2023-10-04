using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoginMenuManager : MonoBehaviour
{
    public static LoginMenuManager loginMenuManager { get; private set; }

    private void Awake()
    {
        if (loginMenuManager != null && loginMenuManager != this)
        {
            Destroy(this);
        }
        else
        {
            loginMenuManager = this;
        }
    }

    #region Variables
    [SerializeField] GameObject loginPage, createPage, mainMenuPage, nakamaClient;
    [SerializeField] TMP_InputField loginEmailInput, createEmailInput, loginPasswordInput, createPasswordInput, usernameInput;
    public TMP_Text loginErrorMessage, createErrorMessage;
    #endregion

    public void openLoginPage()
    {
        createPage.SetActive(false);
        mainMenuPage.SetActive(false);
        loginPage.SetActive(true);
    }
    
    public void openCreatePage()
    {
        createPage.SetActive(true);
        mainMenuPage.SetActive(false);
        loginPage.SetActive(false);
    }

    public void backAPage()
    {
        createPage.SetActive(false);
        mainMenuPage.SetActive(true);
        loginPage.SetActive(false);
    }

    public void LoginButton()
    {
        nakamaClient.GetComponent<NakamaCient>().Login(loginEmailInput.text, loginPasswordInput.text);
        
    }

    public void CreateButton()
    {
        nakamaClient.GetComponent<NakamaCient>().Create(createEmailInput.text, createPasswordInput.text, usernameInput.text);

    }

    public void NextMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
