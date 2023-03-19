using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickNDrag : MonoBehaviour
{
    private GameObject selectedObject;

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
            if (targetObject && !selectedObject)
            {
                selectedObject = targetObject.transform.gameObject;
                selectedObject.GetComponent<BookStack>().SetColliderStatus(false);
            }
        }

        if (Input.GetMouseButtonUp(0) && selectedObject)
        {
            selectedObject.GetComponent<BookStack>().SetColliderStatus(true);
            selectedObject = null;
        }

        if (selectedObject)
        {
            selectedObject.transform.position = mousePosition;
        }
    }
}
