using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookshelfManager : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private Transform[] movePoints;
    private Vector3[] movePos;
    private Vector3 targetPos;
    private int targetIndex;
    private int lastIndex;

    // Start is called before the first frame update
    void Start()
    {   
        List<Vector3> pos = new List<Vector3>();
        foreach (Transform movePoint in movePoints)
        {
            pos.Add(movePoint.position);
        }
        movePos = pos.ToArray();

        lastIndex = movePos.Length - 1;
        transform.position = movePoints[movePos.Length / 2].position;
        targetIndex = movePos.Length / 2;
    }

    public void ArrowClicked(int value)
    {
        targetIndex += value;
        if (targetIndex < 0) targetIndex = lastIndex;
        else if (targetIndex > lastIndex) targetIndex = 0;
        SetNewTargetPos();
    }

    void SetNewTargetPos()
    {
        targetPos = movePos[targetIndex];
        StartCoroutine(MoveShelves());
    }

    IEnumerator MoveShelves()
    {
        int boostSpeed;
        if (Vector2.Distance(transform.position, targetPos) > 1500) 
            boostSpeed = 3;
        else 
            boostSpeed = 1;

        while (Vector2.Distance(transform.position, targetPos) > 1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, (speed * boostSpeed) * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
