using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookClickNDrag : MonoBehaviour
{
    private GameObject selectedObject;
    private bool isPlayable = true;

    public void StopInput()
    {
        isPlayable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        // If the game is playable and there is at least one touch input
        if (!isPlayable || Input.touchCount == 0)
        {
            return;
        }

        // Get the touch input and its position
        Touch touch = Input.GetTouch(0);
        Vector3 inputPosition = touch.position;

        // If the touch event begins
        if (touch.phase == TouchPhase.Began)
        {
            // Get the object the touch is over
            GameObject targetObject = GetObjectAtPosition(inputPosition);

            // If the object is a BookStack and is interactable
            if (targetObject != null && targetObject.CompareTag("BookStacks") && targetObject.GetComponent<BookStack>().interactable)
            {
                // Set the selected object to the touched BookStack
                selectedObject = targetObject.transform.gameObject;
            }
        }
        // If the touch event ended and a BookStack is selected
        else if (touch.phase == TouchPhase.Ended && selectedObject)
        {
            // Call the BookClicked method on the selected BookStack
            selectedObject.GetComponent<BookStack>().BookClicked();
            selectedObject = null;
        }

        if (selectedObject && !StackCover.instance.isOpen && isPlayable)
        // If a Bookstack is selected and the stack cover is closed
        if (selectedObject != null && !StackCover.instance.isOpen)
        {
            // Move the selected BookStack to the touch position
            selectedObject.transform.position = inputPosition;
        }
    }

    // Return the object at the specified position, if any
    private GameObject GetObjectAtPosition(Vector3 position)
    {
        Collider2D targetCollider = Physics2D.OverlapPoint(position);
        Debug.Log(targetCollider?.name);
        return targetCollider?.gameObject;
    }
    
}
