using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraAssign : NetworkBehaviour
{
    public CinemachineFreeLook cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AssignCamera()
    {
        if (isOwned)
        {
            cam = CinemachineFreeLook.FindObjectOfType<CinemachineFreeLook>();
            cam.LookAt = transform;
            cam.Follow = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isLocalPlayer) return; 
        if (cam) return; 
        if (SceneManager.GetActiveScene().name != "GameScene") return;
        cam = CinemachineFreeLook.FindObjectOfType<CinemachineFreeLook>();
        cam.LookAt = transform;
        cam.Follow = transform;
    }
}
