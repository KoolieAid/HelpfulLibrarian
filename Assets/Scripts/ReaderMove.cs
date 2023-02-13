using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaderMove : MonoBehaviour
{
    public float speed;

    public float initialPosOffSet;
    public float finalPosX;

    public bool isStoped;
    
    private bool go;

    void Start()
    {
        transform.position = new Vector3(initialPosOffSet, transform.position.y, transform.position.z);
        isStoped = false;
        go = true;
        
        
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
                //Debug.Log(readerManager.particles.ContainsKey("Star"));
                ReaderManager.Instance.particles["Star"].Play();
            }
            
        }

    }
}
