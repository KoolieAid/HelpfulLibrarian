using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Reader : MonoBehaviour
{
    [SerializeField] private GameObject dialog;
    [SerializeField] private Sprite[] referenceImages;
    [SerializeField] private Image imageComp;

    private float _patience = 100f;

    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            _patience -= 10f;
        }
    }

    public void ShowHideRequest(bool b)
    {
        dialog.SetActive(b);
    }

    public void SetRequest(int index)
    {
        imageComp.sprite = referenceImages[index];
    }

    public void RandomRequest()
    {
        imageComp.sprite = referenceImages[Random.Range(0, referenceImages.Length)];
    }
}
