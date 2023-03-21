using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickNDrag : MonoBehaviour
{
    private GameObject selectedObject;
<<<<<<< HEAD
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
=======
>>>>>>> 9835f7587c216599e86b832458db9a9f7b94088c

    // Update is called once per frame
    void Update()
    {
        Vector3 inputPosition = Input.mousePosition;

<<<<<<< HEAD
        if (Input.GetMouseButtonDown(0) && isPlayable)
        {
            Collider2D targetObject = Physics2D.OverlapPoint(inputPosition);
            if (targetObject && !selectedObject && targetObject.CompareTag("BookStacks") && targetObject.GetComponent<BookStack>().interactable)
=======
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(inputPosition);
            if (targetObject && !selectedObject && targetObject.CompareTag("BookStacks"))
>>>>>>> 9835f7587c216599e86b832458db9a9f7b94088c
            {
                selectedObject = targetObject.transform.gameObject;
            }
        }

        if (Input.GetMouseButtonUp(0) && selectedObject)
        {
            selectedObject.GetComponent<BookStack>().BookClicked();
            selectedObject = null;
        }

<<<<<<< HEAD
        if (selectedObject && !StackCover.instance.isOpen && isPlayable)
=======
        if (selectedObject && !StackCover.instance.isOpen)
>>>>>>> 9835f7587c216599e86b832458db9a9f7b94088c
        {
            selectedObject.transform.position = inputPosition;
        }
    }
}
