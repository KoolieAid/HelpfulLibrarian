using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickNDrag : MonoBehaviour
{
    private GameObject selectedObject;
    private bool isPlayable = true;
    void OnEnable()
    {
        SortingGameManager.OnSortAll += StopInput;
    }
    void OnDisable()
    {
        SortingGameManager.OnSortAll -= StopInput;
    }

    void StopInput(int value1, int value2)
    {
        isPlayable = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 inputPosition = Input.mousePosition;

        if (Input.GetMouseButtonDown(0) && isPlayable)
        {
            Collider2D targetObject = Physics2D.OverlapPoint(inputPosition);
            if (targetObject && !selectedObject && targetObject.CompareTag("BookStacks") && targetObject.GetComponent<BookStack>().interactable)
            {
                selectedObject = targetObject.transform.gameObject;
            }
        }

        if (Input.GetMouseButtonUp(0) && selectedObject)
        {
            selectedObject.GetComponent<BookStack>().BookClicked();
            selectedObject = null;
        }

        if (selectedObject && !StackCover.instance.isOpen && isPlayable)
        {
            selectedObject.transform.position = inputPosition;
        }
    }
}
