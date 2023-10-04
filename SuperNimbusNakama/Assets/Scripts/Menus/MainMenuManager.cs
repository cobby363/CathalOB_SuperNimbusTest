using System.Collections;
using System.Collections.Generic;
using TMPro;
using Mirror;
using UnityEngine.UI;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    #region Variables
    [SerializeField] GameObject MainMenu, NewGame, Settings, JoinGame;
    
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        MainMenu.SetActive(true);
        NewGame.SetActive(false);
        Settings.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplicationQuit()
    {
        Application.Quit();
    }

    public void NewGameButtonPressed()
    {
        MainMenu.SetActive(false);
        NewGame.SetActive(true);
        Settings.SetActive(false);
    }

    public void SettingsButtonPressed()
    {
        MainMenu.SetActive(false);
        NewGame.SetActive(false);
        Settings.SetActive(true);
    }

    public void NewGameBackButtonPressed()
    {
        MainMenu.SetActive(true);
        NewGame.SetActive(false);
        Settings.SetActive(false);
    }

    public void HostLobby()
    {
        NetworkManager.singleton.StartHost();
    }

    public void Join()
    {
        MainMenu.SetActive(false);
        NewGame.SetActive(false);
        JoinGame.SetActive(true);
        
    }

    //private void HandleClientConnected()
    //{
    //    joinButton.interactable = true;

    //    landingPagePanel.SetActive(false);
    //}

    //private void HandleClientDisconnected()
    //{
    //    joinButton.interactable = true;
    //}
}
