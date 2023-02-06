using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class Reader : MonoBehaviour
{
    [SerializeField] private GameObject dialog;
    [SerializeField] private Image imageComp;
    [SerializeField] public Image face;
    public string requestedTitle;

    public float initialPatience;
    private float currentPatience;
    [SerializeField]private Image patienceMeterFill;



    private IEnumerator Start()
    {
        currentPatience = initialPatience;

        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            currentPatience -= 1f;
            patienceMeterFill.fillAmount = (((currentPatience - 0f) * (1f - 0f)) / (initialPatience - 0f)) + 0f;

        }
    }

    public void ShowHideRequest(bool b)
    {
        dialog.SetActive(b);
    }

    public void SetRequestImage(Sprite img)
    {
        imageComp.sprite = img;
    }
}
