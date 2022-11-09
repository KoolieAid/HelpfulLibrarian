using System;
using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private UserInputSequence us;
    private UserInputSequence bookPress;
    private UserInputSequence bookClose;
    private UserInputSequence bookSelected;
    private UserInputSequence confirm;
    
    [SerializeField] private ToolTipAdapter _adapter;
    [SerializeField] private Reader Chichay;
    [SerializeField] private GameObject endPopup;
    
    /*
     * 1. Introduction
     * 2. Visitor Requests (Tooltip)
     * 3. Visitor Patience Meter
     * 4. Book Mechanics and Controls
     * 5. Scoring (Tooltip)
     */

    private void Awake()
    {
        var controller = GetComponent<SequenceController>();
        
        us = new UserInputSequence(controller);
        bookPress = new UserInputSequence(controller);
        bookClose = new UserInputSequence(controller);
        bookSelected = new UserInputSequence(controller);
        confirm = new UserInputSequence(controller);
        
        var rectTransform = GetComponent<RectTransform>();
        
        controller.AddSequence(new WaitSequence(controller, 3.0f))
            // Visitor Request
            .AddSequence(new CustomSequence(controller, (s, o) =>
            {
                Chichay.ShowHideRequest(true);
                s.SetStatus(true);
            }))
            .AddSequence(new MoveSequenceCanvas(controller, new Vector2(254, 191f), 5, rectTransform)) 
            .AddSequence(new WaitSequence(controller, 1.5f))
            .AddSequence(new ToolTipSequence(controller, _adapter, "Ito ang hinihingi ng bisita, konektado ito sa librong gusto niya. \n \n This is the visitor's request, this picture is the book that the visitor wants."))
            .AddSequence(us)
            .AddSequence(new ToolTipSequence(controller, _adapter, "", false))

            // Patience Meter
            .AddSequence(new WaitSequence(controller, 0.5f))
            .AddSequence(new MoveSequenceCanvas(controller, new Vector2(-136, 270f), 5, rectTransform)) 
            .AddSequence(new WaitSequence(controller, 1.5f))
            .AddSequence(new ToolTipSequence(controller, _adapter, "Ito ang 'Patience Meter'. Ibigay ang librong hinihingi bago maubos ang Patience Meter. \n \n This is the 'Patience Meter'. Give the right book before the patience meter runs out."))
            .AddSequence(us)
            .AddSequence(new ToolTipSequence(controller, _adapter, "", false))
            
            // Book Mechanics
            .AddSequence(new WaitSequence(controller, 0.5f))
            .AddSequence(new MoveSequenceCanvas(controller, new Vector2(-340, -555f), 5, rectTransform)) 
            .AddSequence(new WaitSequence(controller, 1.0f))
            .AddSequence(new ToolTipSequence(controller, _adapter, "Pindutin ang libro para makita ang deskripsyon. \n \n When a book is tapped, its description will appear."))
            .AddSequence(us)
            .AddSequence(new ToolTipSequence(controller, _adapter, "", false))
            .AddSequence(bookPress) 
            // .AddSequence(new ToolTipSequence(controller, _adapter, "Pindutin ang naka-highlight upang makita ang deskripsyon ng salita. \n \n Tap the highlighted word to show its description."))
            // .AddSequence(us)
            .AddSequence(new WaitSequence(controller, 1.0f))
            .AddSequence(new ToolTipSequence(controller, _adapter, "Pindutin ang \"X\" upang isara ang libro. \n \n Tap the \"X\" button of the book to close it."))
            .AddSequence(us)
            .AddSequence(new ToolTipSequence(controller, _adapter, "", false))
            .AddSequence(bookClose)
            .AddSequence(new WaitSequence(controller, 0.5f))
            .AddSequence(new ToolTipSequence(controller, _adapter, "Pindutin ng dalawang beses ang libro para piliin ito. \n \n Double tap to select the book."))
            .AddSequence(us)
            .AddSequence(new ToolTipSequence(controller, _adapter, "", false))
            .AddSequence(bookSelected)
            .AddSequence(new WaitSequence(controller, 1.5f))
            
            // need to differentiate the bookSelected and bookPress
            
            .AddSequence(new ToolTipSequence(controller, _adapter, "Pindutin ang 'Confirm' upang ibigay ang libro sa bisita. \n \n Tap 'Confirm' to give the book to the visitor."))
            .AddSequence(us)
            .AddSequence(new ToolTipSequence(controller, _adapter, "", false))

            // should be using the confirm instead of the next button
            .AddSequence(confirm)
            .AddSequence(new WaitSequence(controller, 1.0f))

            .AddSequence(new ToolTipSequence(controller, _adapter, "", false))

            // Scoring
            .AddSequence(new MoveSequenceCanvas(controller, new Vector2(397, 706f), 5, rectTransform)) 
            .AddSequence(new WaitSequence(controller, 2.0f))
            .AddSequence(new ToolTipSequence(controller, _adapter, "Sa simula, mayroon kang tatlong star. Mababawasan ang star kapag may umalis na bista. \n \n You start with 3 stars every level. A star would be removed if a visitor leaves."))
            .AddSequence(us)
            .AddSequence(new ToolTipSequence(controller, _adapter, "Kapag naubos ang tatlong star, ikaw ay matatalo. \n \n You lose if you run out of stars."))
            .AddSequence(us)
            .AddSequence(new ToolTipSequence(controller, _adapter, "", false))
            
            // end tutorial, go to game mode select scene
            .AddSequence(new WaitSequence(controller, 1.0f))
            .AddSequence(new CustomSequence(controller, (sequence, o) =>
            {
                endPopup.SetActive(true);
                sequence.SetStatus(true);
            }))

            .AddSequence(new CustomSequence(controller)
                .SetAction((s, o) =>
                {
                    // o.SetActive(false);
                    Debug.Log("Tutorial end");
                }));

    }

    public void Test()
    {
        us.Toggle();
    }

    public void BookPressed()
    {
        bookPress.Toggle();
    }

    public void BookClosed()
    {
        bookClose.Toggle();
    }

    public void BookSelected()
    {
        bookSelected.Toggle();
    }

    public void ComfirmPressed()
    {
        confirm.Toggle();
    }
}