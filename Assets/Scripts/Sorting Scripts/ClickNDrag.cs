using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickNDrag : MonoBehaviour
{
    public GameObject selectedObject;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
            if (targetObject)
            {
                selectedObject = targetObject.transform.gameObject;
            }
        }

        if (Input.GetMouseButtonUp(0) && selectedObject)
        {
            selectedObject = null;
        }

        if (selectedObject)
        {
            selectedObject.transform.position = mousePosition;
        }
    }
}
