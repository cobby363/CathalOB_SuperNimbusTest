using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{

    [SerializeField] string lobbyAddress;

    private void OnEnable()
    {
        COBNetworkManager.ClientOnConnected += HandleClientConnected;
        COBNetworkManager.ClientOnDisconnected += HandleClientDisconnected;
        StartCoroutine("JoiningCoroutine");

    }

    private void OnDisable()
    {
        COBNetworkManager.ClientOnConnected -= HandleClientConnected;
        COBNetworkManager.ClientOnDisconnected -= HandleClientDisconnected;
    }

    public void Join()
    {
        NetworkManager.singleton.networkAddress = lobbyAddress;
        NetworkManager.singleton.StartClient();

        
    }

    IEnumerator JoiningCoroutine()
    {
        yield return new WaitForSeconds(3);
        Join();
        gameObject.SetActive(false);
        StopAllCoroutines();
    }

    private void HandleClientConnected()
    {

    }

    private void HandleClientDisconnected()
    {

    }
}
