using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookStack : MonoBehaviour
{

    public Topics category;
    private Vector2 originalPos;
    public int speed = 300;
    private Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        rigidbody = GetComponent<Rigidbody2D>();
    }


    void OnEnable()
    {
        Bookshelf.OnSort += Sorted;
    }
    void OnDisable()
    {
        Bookshelf.OnSort -= Sorted;
    }

    void Sorted(bool status)
    {
        if (status)
        {

        }
        else if (!status)
        {
            StartCoroutine("ReturnToStartPos");
        }
    }

    public void SetColliderStatus(bool isActive)
    {
        GetComponent<Collider2D>().enabled = isActive;
    }
    IEnumerator ReturnToStartPos()
    {
        while (Vector2.Distance(transform.position, originalPos) > 1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPos, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
