using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReaderMove : MonoBehaviour
{
    public float speed;

    private RectTransform readerPos;
    private RectTransform movePosPoint;

    void Start()
    {
        readerPos = GetComponent<RectTransform>();
        movePosPoint = (RectTransform)gameObject.transform.root.transform.Find("Requester Point");

        Reader.Instance.canDeduct = false;

        StartCoroutine(MoveReader());
    }

    IEnumerator MoveReader()
    {
        // var movePos = new Vector2(280, 129);
        var movePos = new Vector2(movePosPoint.position.x, movePosPoint.position.y);
        while (Vector2.Distance(readerPos.position, movePos) > 1f)
        {
            
            readerPos.position = Vector2.MoveTowards(readerPos.position, movePos, speed * Time.deltaTime);
            
            yield return new WaitForEndOfFrame();
        }
        Reader.Instance.canDeduct = true;
        ParticleManager.Instance.PlayParticle("Star");
        Audio.Instance.PlaySfx("NewVisitor");
    }
}