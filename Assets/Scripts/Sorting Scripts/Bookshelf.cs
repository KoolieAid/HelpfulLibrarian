using System.Collections;
using System.Collections.Generic;
using UnityEngine;
   public enum Topics 
    {
        Topic1,
        Topic2,
        Topic3,
        Topic4,
        Topic5,
        Topic6,
        Topic7,
    };
public class Bookshelf : MonoBehaviour
{
 
    [SerializeField] private Topics category;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TriggerEnter");
        if (collision.gameObject.GetComponent<BookStack>().category == category)
        {
            Debug.Log("SAME");
        }
        else
        {
            Debug.Log("DIFF");
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
