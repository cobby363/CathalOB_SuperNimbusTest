using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach(Player player in GameObject.Find("NetworkManager").GetComponent<COBNetworkManager>().players)
        {
            if (player != null)
            {
                if(gameObject.name == "StartTimeText")
                    player.startTime = this.GetComponent<TMP_Text>();
                else if( gameObject.name == "FinishTimeText")
                    player.finalRaceTimeText = this.GetComponent<TMP_Text>();
                else if(gameObject.name == "PlayerHasFinishedText")
                    player.playerFinishedRaceText = this.GetComponent<TMP_Text>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
