using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReaderMove : MonoBehaviour
{
    public float speed;

    public float initialPosOffSet;
    public float finalPosX;

    public bool isStoped;

    void Start()
    {
        transform.position = new Vector3(initialPosOffSet, transform.position.y, transform.position.z);
        isStoped = false;

        StartCoroutine(MoveReader());
    }
    void Update()
    {

    }

    IEnumerator MoveReader()
    {
        while (transform.position.x < finalPosX)
        {
            //transform.Translate(Vector3.right * speed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(finalPosX, transform.position.y, transform.position.z), speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        isStoped = true;
        ReaderManager.Instance.particles["Star"].Play();

    }
}
