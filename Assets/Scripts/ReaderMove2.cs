using System;
using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

public class ReaderMove2 : MonoBehaviour
{
    private void Awake()
    {
        var controller = GetComponent<SequenceController>();
        var rectTransform = GetComponent<RectTransform>();

        this.transform.position = new Vector3(-125, 0, 0);
        
        controller.AddSequence(new WaitSequence(controller, 3.0f))
            
            // reader should be outside the frame
            // 
            .AddSequence(new MoveSequenceCanvas(controller, new Vector2(0, 0), 700, rectTransform));
    }
    
}
