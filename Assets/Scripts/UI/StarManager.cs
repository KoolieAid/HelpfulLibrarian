using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : MonoBehaviour
{
    public GameObject[] stars = new GameObject[3];

    public void SetStarsToActive(int numOfStars)
    {
        for (int i = 0; i < numOfStars; i++) {
            stars[i].SetActive(true);
        }
    }

}
