using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class COBNetworkManager : NetworkManager
{

    public static event Action ClientOnConnected;
    public static event Action ClientOnDisconnected;

    private bool isGameInProgress = false;

    public List<Player> players = new List<Player>();
    [SerializeField] float countdownTime;
    public static double raceStartTime = 0;

    public int finishedPlayers = 0;



    #region Server Methods
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if (isGameInProgress) { 
            conn.Disconnect();
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        Player player = conn.identity.GetComponent<Player>();

        players.Remove(player);

        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        players.Clear();

        isGameInProgress = false;
    }

    public void StartGame()
    {
        if (players.Count < 2) { return; }

        isGameInProgress = true;

        ServerChangeScene("GameScene");
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        Player player = conn.identity.GetComponent<Player>();

        players.Add(player);

        player.SetPlayerColour(new Color(
            UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f)
        ));

        player.SetPartyOwner(players.Count == 1);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (SceneManager.GetActiveScene().name.Equals("GameScene"))
        {
            finishedPlayers = 0;
            StartCoroutine("CountdownCoroutine");
        }
    }

    //countdown to race starting
    public IEnumerator CountdownCoroutine()
    {
        float remainingTime = countdownTime;
        int currentCountDown = (int)countdownTime;
        

        while (remainingTime >= -3)
        {
            yield return null;

            int newCeilTime = (int)MathF.Ceiling(remainingTime);
            if(newCeilTime != currentCountDown)
            {                
                foreach (Player player in players)
                {
                    player.RpcUpdateCountdown(currentCountDown);
                }
                currentCountDown = newCeilTime;
                Debug.Log("Current Count Down 2: " + currentCountDown);
            }
                remainingTime -= Time.deltaTime;
        }
        raceStartTime = NetworkTime.time;
        StopAllCoroutines();
    }
    #endregion

    #region Client Methods
    public override void OnClientConnect()
    {
        base.OnClientConnect();
        ClientOnConnected?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        ClientOnDisconnected?.Invoke();
    }


    public override void OnStopClient()
    {
        players.Clear();
    }

    #region Race End

    public void PlayerFinishedRace()
    {
        finishedPlayers++;
        Debug.Log("Running playerfinished race. finished racer number is: " + finishedPlayers);
        
        if(finishedPlayers == players.Count)
        {
            StartCoroutine("WaitToEnd");
        }
    }

    //give players enough time to view their time
    IEnumerator WaitToEnd()
    {
        yield return new WaitForSeconds(3);
        ServerChangeScene("Leaderboards");
        StopAllCoroutines();
    }
    #endregion
    #endregion
}
