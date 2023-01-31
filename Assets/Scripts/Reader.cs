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

    public void SetRequestImage(Sprite img)
    {
        imageComp.sprite = img;
    }
}
