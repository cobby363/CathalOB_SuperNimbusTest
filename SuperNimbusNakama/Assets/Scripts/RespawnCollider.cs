using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered");
        if (other.gameObject.tag.Equals("PlayerRacing"))
        {
            Debug.Log("Entered if");
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.transform.position = other.gameObject.GetComponent<PlayerController>().respawnSpawn;
        }
    }
}
