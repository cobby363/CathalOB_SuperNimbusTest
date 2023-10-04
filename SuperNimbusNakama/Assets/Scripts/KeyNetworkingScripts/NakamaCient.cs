using System.Collections;
using System;
using System.Collections.Generic;
using Nakama;
using UnityEngine.SceneManagement;
using UnityEngine;

public class NakamaCient : MonoBehaviour
{

    public readonly IClient client = new Client("http", "127.0.0.1", 7350, "defaultkey");
    public string storedSessionInfo;
    public string localUID = string.Empty;
    public ISession storedSession;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("NakamaClient");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteAll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void Login(string Email, string Password)
    {
        try { 
            var session = await client.AuthenticateEmailAsync(Email, Password, null, false); 
            storedSessionInfo = session.ToString();
            Debug.Log(session);
            storedSession = session;
            PlayerPrefs.SetString("PlayerUsername", session.Username);
            SceneManager.LoadScene("MainMenu");
        }
        catch(Exception ex)
        {
            Debug.Log("error"); 
            Debug.Log(ex.Message.ToString());
            LoginMenuManager.loginMenuManager.loginErrorMessage.text = ex.Message + "Please try again";
        }
    }

    public async void Create(string Email, string Password, string Username)
    {
        try
        {
            var session = await client.AuthenticateEmailAsync(Email, Password, Username);
            storedSessionInfo = session.ToString();
            Debug.Log(session);
            storedSession = session;
            PlayerPrefs.SetString("PlayerUsername", session.Username);
            SceneManager.LoadScene("MainMenu");
        }catch(Exception ex)
        {
            Debug.Log("error");
            Debug.Log(ex.Message.ToString());
            LoginMenuManager.loginMenuManager.createErrorMessage.text = ex.Message + "Please try again";
        }
        
    }
}
