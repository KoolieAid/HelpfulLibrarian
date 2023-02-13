using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaderMove : MonoBehaviour
{
    public float speed;

    public float initialPosOffSet;
    public float finalPosX;


    private void Start()
    {
        Debug.Log("Reader Start");
        NewReader();
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < finalPosX)
            transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    public void NewReader()
    {
        transform.position = new Vector3(initialPosOffSet, transform.position.y, transform.position.z);
    }
}
