using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager gameManager { get; private set; }

    private void Awake()
    {
        if (gameManager != null && gameManager != this)
        {
            Destroy(this);
        }
        else
        {
            gameManager = this;
        }
    }
    #endregion

    #region Variables
    public bool startTimerExpired, raceStarted = false;
    public float startTimer = 6f;
    public TMP_Text startTime;
    public Rigidbody playerRigidbody;

    public float RaceTimer = 0f;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody.isKinematic = true;
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!startTimerExpired)
        {
            startTimer -= Time.deltaTime;
            if (startTime.text != "GO!")
            {
                startTime.text = Mathf.Ceil(startTimer - 1) + "";
            }
            if(startTimer <= 1 && (startTimer > 0))
            {
                startTime.text = "GO!";
            }
            if(startTimer <= 0)
            {
                startTime.text = "";
                startTimerExpired = true;     
                playerRigidbody.isKinematic = false;
                Camera.main.GetComponent<CinemachineBrain>().enabled = true;
                raceStarted = true;
            }
        }
        if (raceStarted)
        {
            RaceTimer += Time.deltaTime;
        }
    }
}
