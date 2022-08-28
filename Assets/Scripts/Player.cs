using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject Pickaxe;
    public float _floatTimer = 0;
    private int _axeArc = 3;
    private int _speed = 10;
    private int _frameRotation = 5;

    void FixedUpdate()
    {
        _floatTimer += Time.deltaTime;

        if(Input.GetMouseButton(0)){
            Pickaxe.transform.Rotate(0, 0, _frameRotation * Mathf.Cos(_floatTimer*_speed)/_axeArc);
        }
        
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 5);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray);

            for (int i = 0; i < hits.Length; i++){
                RaycastHit hit = hits[i];

                if(hit.transform.GetComponent<Player>() == null){
                    Renderer rend = hit.transform.GetComponent<Renderer>();

                    if (rend)
                    {
                        rend.material.shader = Shader.Find("Transparent/Diffuse");
                        Color tempColor = rend.material.color;
                        tempColor.a = 0.3F;
                        rend.material.color = tempColor;
                        break;
                    }
                }
            }
        }
    }
}
