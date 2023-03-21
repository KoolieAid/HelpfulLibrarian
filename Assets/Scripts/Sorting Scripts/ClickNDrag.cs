using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickNDrag : MonoBehaviour
{
    private GameObject selectedObject;

    // Update is called once per frame
    void Update()
    {
        Vector3 inputPosition = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(inputPosition);
            if (targetObject && !selectedObject && targetObject.CompareTag("BookStacks"))
            {
                selectedObject = targetObject.transform.gameObject;
            }
        }

        if (Input.GetMouseButtonUp(0) && selectedObject)
        {
            selectedObject.GetComponent<BookStack>().BookClicked();
            selectedObject = null;
        }

        if (selectedObject && !StackCover.instance.isOpen)
        {
            selectedObject.transform.position = inputPosition;
        }
    }
}
