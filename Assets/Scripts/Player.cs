using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject Pickaxe;
    public GameObject Crosshair;
    public float _floatTimer = 0;
    private int _axeArc = 3;
    private int _speed = 10;
    private int _frameRotation = 5;
    public float CrosshairDistance = 2.5f;
    private bool _mining = false;

    void Update()
    {
        _floatTimer += Time.deltaTime;

        if(Input.GetMouseButton(0)){
            Pickaxe.transform.Rotate(0, 0, _frameRotation * Mathf.Cos(_floatTimer*_speed)/_axeArc);
        }
        
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * CrosshairDistance);

        Crosshair.transform.position = Camera.main.transform.position + Camera.main.transform.forward * CrosshairDistance;

        if (Input.GetMouseButtonDown(0))
        {
            int layer_mask = LayerMask.GetMask("Terrain");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * CrosshairDistance, Color.yellow, 1f);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray, CrosshairDistance, layer_mask);
            if(hits.Length > 0 && !_mining){
                _mining = true;
                Destroy(hits[0].collider.gameObject); //Needs to be sorted?
                GameObject go =  new GameObject();
                go.transform.position = Vector3Int.FloorToInt(hits[0].transform.position);
                GameManager.Instance.Blocks[Vector3Int.FloorToInt(hits[0].transform.position)] = go;
            }
        }else{
            _mining = false;
        }
    }
}
