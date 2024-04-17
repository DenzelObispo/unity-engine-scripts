using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    GameObject target;
    Vector3 playerPosition;

    void Update()
    {
        //transform.LookAt(Camera.main.transform.position, Vector3.forward);
        target = GameObject.FindWithTag("Player");

        playerPosition = new Vector3(target.transform.position.x,
                                        transform.position.y,
                                        target.transform.position.z);
        transform.LookAt(playerPosition);
    }
}
