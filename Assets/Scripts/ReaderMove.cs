using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReaderMove : MonoBehaviour
{
    public float speed;

    [SerializeField] private Vector2 initialPosOffSet;

    private RectTransform readerPos;

    void Start()
    {
        readerPos = GetComponent<RectTransform>();
        readerPos.anchoredPosition = initialPosOffSet;
        
        Reader.Instance.canDeduct = false;

        StartCoroutine(MoveReader());
    }

    IEnumerator MoveReader()
    {
        var movePos = new Vector2(280, 129);
        while (Vector2.Distance(readerPos.anchoredPosition, movePos) > 1f)
        {
            readerPos.anchoredPosition = Vector3.MoveTowards(readerPos.anchoredPosition, movePos, speed * Time.deltaTime);
            
            yield return new WaitForEndOfFrame();
        }
        Reader.Instance.canDeduct = true;
        ReaderManager.Instance.particles["Star"].Play();
    }
}
