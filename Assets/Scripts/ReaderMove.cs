using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaderMove : MonoBehaviour
{
    public float speed;

    public float initialPosOffSet;
    public float finalPosX;

    public bool isStoped;

    private ParticleSystem sparkle;
    private bool go;

    void Start()
    {
        transform.position = new Vector3(initialPosOffSet, transform.position.y, transform.position.z);
        isStoped = false;
        go = true;

        sparkle = GameObject.Find("star").GetComponent<ParticleSystem>();
        
    }
    void Update()
    {
        if (transform.position.x < finalPosX)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            isStoped = false;
        }
        else
        {
            isStoped = true;
            if (go)
            {
                go = false;
                sparkle.Play();
            }
            
        }

    }
}
