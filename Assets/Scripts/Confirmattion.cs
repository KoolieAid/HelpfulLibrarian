using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Confirmattion : MonoBehaviour
{
    public static Confirmattion Instance;
    public UnityEvent onConfirm = new();


    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        transform.parent.gameObject.SetActive(false);
    }

    public void CloseConfirmation()
    {
        onConfirm.RemoveAllListeners();
        transform.parent.gameObject.SetActive(false);
    }

    public void Confirm()
    {
        onConfirm.Invoke();
    }
}
