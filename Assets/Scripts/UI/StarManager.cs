using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : MonoBehaviour
{
    public GameObject[] stars = new GameObject[3];
    


    public void SetStarsToActive(int numOfStars)
    {
        if (numOfStars > 0) stars[0].SetActive(true);
        if (numOfStars > 1) stars[1].SetActive(true);
        if (numOfStars > 2) stars[2].SetActive(true);
    }

}
