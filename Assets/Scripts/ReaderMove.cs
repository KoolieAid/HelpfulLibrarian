using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReaderMove : MonoBehaviour
{
    public float speed;

    private RectTransform readerPos;

    void Start()
    {
        readerPos = GetComponent<RectTransform>();

        Reader.Instance.canDeduct = false;

        StartCoroutine(MoveReader());
    }

    IEnumerator MoveReader()
    {
        // var movePos = new Vector2(280, 129);
        var movePos = new Vector2(-286, 129);
        while (Vector2.Distance(readerPos.anchoredPosition, movePos) > 1f)
        {
            readerPos.anchoredPosition = Vector3.MoveTowards(readerPos.anchoredPosition, movePos, speed * Time.deltaTime);
            
            yield return new WaitForEndOfFrame();
        }
        Reader.Instance.canDeduct = true;
        ParticleManager.Instance.PlayParticle("Star");
        Audio.Instance.newVisitorSfx.Play();
    }
}
