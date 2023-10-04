using Cinemachine;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : NetworkBehaviour
{
    #region Variables

    [SyncVar(hook = nameof(ClientHandleDisplayNameUpdated))]
    public string displayName = "Blank";
    [SyncVar(hook = nameof(AuthorityHandlePartyOwnerStateUpdated))]
    private bool isPartyOwner = false;
    [SyncVar] public string playerUname = null;
    [SyncVar] public bool playerReady = false;
    private bool startTimerExpired, raceStarted = false;
    public TMP_Text startTime, finalRaceTimeText, playerFinishedRaceText;
    float finalRaceTime = 0;
    [SerializeField] public Rigidbody rb;
    [SerializeField] CameraAssign camAssign;
    [SerializeField] NetworkRigidbodyUnreliable networkRb = null;

    public static event Action ClientOnInfoUpdated;
    public static event Action<bool> AuthorityOnPartyOwnerStateUpdated;

    private Color playerColour = new Color();
    #endregion


   

    private void Start()
    {
        if (!isOwned) return;
        CmdSendName(PlayerPrefs.GetString("PlayerUsername"));
        SceneManager.sceneLoaded += OnSceneLoaded;
        rb  = GetComponent<Rigidbody>();
        networkRb = gameObject.GetComponent<NetworkRigidbodyUnreliable>();
        
    }

    
    private void OnSceneLoaded(Scene scene1, LoadSceneMode scene2)
    {
        if (SceneManager.GetActiveScene().name.Equals("GameScene"))
        {
            this.gameObject.transform.position = new Vector3(-3, 3 + (1.25f * netId), .75f);
            if (!isOwned) return;
            camAssign.AssignCamera();
        }
    }

    [Client]
    public void GetFinalTime()
    {
        if (!isOwned) return;
        finalRaceTime = (float)(NetworkTime.time - COBNetworkManager.raceStartTime); 
        CmdFinishedRace();

        float minutes = Mathf.FloorToInt(finalRaceTime / 60);
        float seconds = Mathf.FloorToInt(finalRaceTime % 60);
        finalRaceTimeText.text = ("Final Time: " + minutes + ":" + seconds);
    }

    public string GetDisplayName()
    {
        return displayName;
    }

    public bool GetIsPartyOwner()
    {
        return isPartyOwner;
    }

    public Color getPlayerColour()
    {
        return playerColour;
    }

    #region Server
    public override void OnStartServer()
    {
        DontDestroyOnLoad(gameObject);
    }

    [Server]
    public void SetDisplayName(string displayName)
    {
        this.displayName = displayName;
    }

    [Server]
    public void SetPlayerColour(Color newPlayerColour)
    {
        playerColour = newPlayerColour;
    }

    [Server]
    public void SetPartyOwner(bool state)
    {
        isPartyOwner = state;
    }

    [Command]
    public void CmdStartGame()
    {
        if (!isPartyOwner) { return; }
        Debug.Log("Player.CmdStartGame");
        ((COBNetworkManager)NetworkManager.singleton).StartGame();
    }
    #endregion

    #region Client
    

    public override void OnStartClient()
    {
        if (NetworkServer.active) { return; }

        DontDestroyOnLoad(gameObject);
        
        ((COBNetworkManager)NetworkManager.singleton).players.Add(this);
    }

    public override void OnStopClient()
    {
        ClientOnInfoUpdated?.Invoke();

        if (!isClientOnly) { return; }

        ((COBNetworkManager)NetworkManager.singleton).players.Remove(this);
    }

    private void ClientHandleDisplayNameUpdated(string oldDisplayName, string newDisplayName)
    {
        ClientOnInfoUpdated?.Invoke();
    }

    private void AuthorityHandlePartyOwnerStateUpdated(bool oldState, bool newState)
    {
        if (!isOwned) { return; }

        AuthorityOnPartyOwnerStateUpdated?.Invoke(newState);
    }

    [Command]
    public void CmdSendName(string name)
    {
        displayName = name;
    }


    [Client]
    public string GetUsernameFromNakama()
    {
        Debug.Log(PlayerPrefs.GetString("PlayerUsername"));
        return PlayerPrefs.GetString("PlayerUsername");
    }

    [ClientRpc]
    public void RpcUpdateCountdown(int countdown)
    {
        if(!isOwned) { return; }
        if (countdown >= 1)
            startTime.text = "" + countdown;
        else if (countdown == 0)
            startTime.text = "GO!";
        else if (countdown < 0)
        {
            startTime.text = "";
            rb.isKinematic = false;
        }

    }

    [Command]
    public void CmdFinishedRace()
    {
        GameObject.Find("NetworkManager").GetComponent<COBNetworkManager>().PlayerFinishedRace();
    }

    [Client]
    public async void SubmitLeaderboardScore()
    {
        if (!isOwned) return;
        NakamaCient nakama = FindObjectOfType<NakamaCient>();
        const string leaderboardId = "global";
        long score = (long)finalRaceTime;
        var r = await nakama.client.WriteLeaderboardRecordAsync(nakama.storedSession, leaderboardId, score);
        Debug.LogFormat("New record for '{0}' score '{1}'", r.Username, r.Score);
    }



    //---------------------------Unsure as to why the below function would never run. Never an error, no debug messages, just never ran-------------------------

    //[ClientRpc]
    //public void RpcAssignInGameUI()
    //{
    //    Debug.Log("AssignInGameUI() Runnint");
    //    //if (isOwned)

    //    Debug.Log("AssignInGameUI() inside ifHJGVBIFVUNDIVNIVNSGTNSUTBGNSUGTNSPITGNUBSRPBNTSPIBUNIPBNRTU");
    //    startTime = GameObject.Find("InGameUI").GetComponentInChildren<TMP_Text>();
    //    //startTimerExpired = GameManager.gameManager.startTimerExpired;
    //    //raceStarted = GameManager.gameManager.raceStarted;

    //}
    #endregion
}
