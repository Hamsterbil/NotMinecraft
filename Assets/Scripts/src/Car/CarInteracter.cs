using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInteracter : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray camRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            if (Physics.Raycast(camRay, out RaycastHit hitInfo, 5f))
            {
                print(hitInfo.collider.gameObject.name);
                Vector3 targetPoint = hitInfo.point - hitInfo.normal * .1f;
                if(hitInfo.collider.gameObject.name == "CarCollider"){
                    hitInfo.collider.transform.parent.gameObject.GetComponent<CarController>().GetIn(this);
                }
            }
        }
    }
}
