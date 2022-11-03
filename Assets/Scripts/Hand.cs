using System;
using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private GameObject toolTipPrefab;

    private void Awake()
    {
        var controller = GetComponent<SequenceController>();

        controller.AddSequence(new WaitSequence(controller, 5.0f))
            .AddSequence(new MoveSequence(controller, new Vector2(0.3f, 0.5f), 0.05f))
            .AddSequence(new WaitSequence(controller, 2.0f))
            .AddSequence(new MoveSequence(controller, new Vector2(1.0f, 2.2f), 0.05f))
            .AddSequence(new ToolTipSequence(controller, toolTipPrefab, "test", 5.0f))
            .AddSequence(new WaitSequence(controller, 2.0f))
            .AddSequence(new CustomSequence(controller)
                .SetAction(o =>
                {
                    o.GetComponent<SpriteRenderer>().enabled = false;
                }));
    }
}