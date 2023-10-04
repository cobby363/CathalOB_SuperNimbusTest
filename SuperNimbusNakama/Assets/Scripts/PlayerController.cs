using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : NetworkBehaviour
{
    #region Variables
    [SerializeField] Rigidbody rb;
    [SerializeField] CameraAssign camAssign;
    [SerializeField] float speed = 1.0f;
    public Vector3 respawnSpawn;
    private float finalRaceTime = 0f;
    [SerializeField] TMP_Text finalRaceTimeText;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if(isOwned)
        {
            //assign cinemachine camera to each player
            camAssign = transform.GetChild(0).GetComponent<CameraAssign>();
        }
        
    }

    //movement
    [ClientCallback]
    private void FixedUpdate()
    {
        if (isOwned)
        {
            if (SceneManager.GetActiveScene().name == "GameScene")
            {
                //inputs
                float moveHorizontal = Input.GetAxis("Horizontal");
                float moveVertical = Input.GetAxis("Vertical");

                //Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
                //camera directions
                Vector3 cameraForward = Camera.main.transform.forward;
                Vector3 cameraRight = Camera.main.transform.right;

                cameraForward.y = 0;
                cameraRight.y = 0;

                //relative cam direction
                Vector3 forwardRelative = moveVertical * cameraForward;
                Vector3 rightRelative = moveHorizontal * cameraRight;

                Vector3 moveDirection = forwardRelative + rightRelative;
                //pushing player with force
                rb.AddForce(moveDirection * speed);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    
}
