using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Confirmattion : MonoBehaviour
{
    public static Confirmattion Instance;
    public UnityEvent onConfirm = new();


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void CloseConfirmation()
    {
        gameObject.SetActive(false);
    }

    public void Confirm()
    {
        onConfirm.Invoke();
    }
}
