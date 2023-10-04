using Mirror.Examples.Tanks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;

public class LeaderboardEntry : MonoBehaviour
{
    #region Variables
    [SerializeField] private TMP_Text rank = null;
    [SerializeField] private TMP_Text username = null;
    [SerializeField] private TMP_Text timeScore = null;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetPlayer(string username, int rank, string score)
    {
        this.username.text = username;
        this.rank.text = rank + ".";
        this.timeScore.text = score;
    }
}
