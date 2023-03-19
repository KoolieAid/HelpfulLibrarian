using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mem_Book : MonoBehaviour
{
	private Camera _camera;
    
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }
	
    void OnMouseOver()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        
        Debug.Log("yo");
        
        // var r = _camera.ScreenPointToRay(Input.mousePosition);
        //
        // if (Physics.Raycast(r, out RaycastHit hit))
        // {
        //     Debug.Log($"{hit.collider.name}");
        // }
        // else
        // {
        //     Debug.Log("wala");
        // }
    }
}
