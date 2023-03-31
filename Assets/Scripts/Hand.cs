using System;
using System.Collections;
using System.Collections.Generic;
using Asyncoroutine;
using Tutorial;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private UserInputSequence us;
    private UserInputSequence bookPress;
    private UserInputSequence bookClose;
    private UserInputSequence bookSelected;
    private UserInputSequence confirm;
    
    [SerializeField] private Text2ToolTipAdapter _adapter;
    [SerializeField] private Reader Chichay;
    [SerializeField] private GameObject endPopup;
    [SerializeField] private Text2ToolTipAdapter startPopup;

    [SerializeField] private GameObject correctBook;
    [SerializeField] private float bookGivingSpeed = 1000f;

    [SerializeField] private GameObject scoreStatusPanel;

    private int handSpeed = 700;

    private Coroutine dragRoutine;
    
    /*
     * 1. Introduction
     * 2. Visitor Requests (Tooltip)
     * 3. Visitor Patience Meter
     * 4. Book Mechanics and Controls
     * 5. Scoring (Tooltip)
     */

    private void Awake()
    {
        Application.targetFrameRate = 60;
        var controller = GetComponent<SequenceController>();
        
        us = new UserInputSequence(controller);
        bookPress = new UserInputSequence(controller);
        bookClose = new UserInputSequence(controller);
        bookSelected = new UserInputSequence(controller);
        confirm = new UserInputSequence(controller);
        
        var rectTransform = GetComponent<RectTransform>();

        controller.AddSequence(new WaitSequence(controller, 2.0f))

            // Narrative Popup
            .AddSequence(new TwoToolTipSequence(controller, _adapter, "Kamusta! ikaw siguro ang bagong librarian.","Oh Hello! You must be the new librarian."))
            .AddSequence(us)
            .AddSequence(new TwoToolTipSequence(controller, _adapter, "Maligayang pagdating sa ating silid-aklatan.","Well then let me be the first to welcome you to the library. "))
            .AddSequence(us)
            .AddSequence(new TwoToolTipSequence(controller, _adapter, "Sasabihin ko sayo ang mga kailangan mong gawin.","I was assigned to help get you all set up. "))
            .AddSequence(us)
            .AddSequence(new TwoToolTipSequence(controller, _adapter, "Ang iyong trabaho ay tulungan ang mga bisita na hanapin ang librong gusto nila.","Your first job will is to help visitors find the books that they want."))
            .AddSequence(us)
            .AddSequence(new TwoToolTipSequence(controller, _adapter, "Ayan! May bago tayong bisita!","Oh look there is our first visitor"))
            .AddSequence(us)
            .AddSequence(new TwoToolTipSequence(controller, _adapter, "Hayaan mong gabayan kita para makita mo ang kailangan mong gawin.","Let me guide you through this one so you can see how it is done."))
            .AddSequence(us)
            .AddSequence(new TwoToolTipSequence(controller, _adapter))
            
            // Visitor Request
            .AddSequence(new CustomSequence(controller, (s, o) =>
            {
                Chichay.gameObject.SetActive(true);
                s.SetStatus(true);
            }))
            .AddSequence(new MoveSequenceCanvas(controller, new Vector2(254, 191f), handSpeed, rectTransform))
            .AddSequence(new WaitSequence(controller, 1.5f))
            .AddSequence(new TwoToolTipSequence(controller, _adapter,
                "Ito ang hinihingi ng bisita, konektado ito sa librong gusto niya.","This is the visitor's request, this picture is the book that the visitor wants."))
            .AddSequence(us)
            .AddSequence(new TwoToolTipSequence(controller, _adapter))

            // Patience Meter
            .AddSequence(new WaitSequence(controller, 0.5f))
            .AddSequence(new MoveSequenceCanvas(controller, new Vector2(-136, 270f), handSpeed, rectTransform))
            .AddSequence(new WaitSequence(controller, 1.5f))
            .AddSequence(new TwoToolTipSequence(controller, _adapter,
                "Ito ang 'Patience Meter'. Ibigay ang librong hinihingi bago maubos ang Patience Meter.","This is the 'Patience Meter'. Give the right book before the patience meter runs out."))
            .AddSequence(us)
            .AddSequence(new TwoToolTipSequence(controller, _adapter))

            // Book Mechanics
            .AddSequence(new WaitSequence(controller, 0.5f))
            .AddSequence(new MoveSequenceCanvas(controller, new Vector2(-340, -555f), handSpeed, rectTransform))
            .AddSequence(new WaitSequence(controller, 1.0f))
            .AddSequence(new TwoToolTipSequence(controller, _adapter,
                "Pindutin ang libro para makita ang deskripsyon.","When a book is tapped, its description will appear."))
            .AddSequence(us)
            .AddSequence(new TwoToolTipSequence(controller, _adapter))
            .AddSequence(bookPress)
            .AddSequence(new WaitSequence(controller, 0.5f))
            .AddSequence(new TwoToolTipSequence(controller, _adapter,
                "Pindutin ang \"X\" upang isara ang libro.","Tap the \"X\" button of the book to close it."))
            .AddSequence(us)
            .AddSequence(new TwoToolTipSequence(controller, _adapter))
            .AddSequence(bookClose)
            .AddSequence(new WaitSequence(controller, 0.5f))
            .AddSequence(new TwoToolTipSequence(controller, _adapter,
                "Dalhin ang librong napili papunta sa bisita.","Drag book to reader to select the book."))
            .AddSequence(us)
            .AddSequence(new TwoToolTipSequence(controller, _adapter))
            
            // Simulate dragging the book
            .AddSequence(new CustomSequence(controller, async (seq, _) =>
            {
                dragRoutine = StartCoroutine(DragRepeat());
                seq.SetStatus(true);
            }))
            
            .AddSequence(bookSelected)
            .AddSequence(new CustomSequence(controller, (seq, _) =>
            {
                StopCoroutine(dragRoutine);
                dragRoutine = null;
                seq.SetStatus(true);
            }))
            .AddSequence(new WaitSequence(controller, 1.0f))

            // need to differentiate the bookSelected and bookPress

            .AddSequence(new TwoToolTipSequence(controller, _adapter,
                "Pindutin ang tsek upang ibigay ang libro sa bisita.","Tap 'Confirm' to give the book to the visitor."))
            .AddSequence(us)
            .AddSequence(new TwoToolTipSequence(controller, _adapter))

            // should be using the confirm instead of the next button
            .AddSequence(confirm)
            .AddSequence(new TwoToolTipSequence(controller, _adapter))
            
            // give correct book
            // Drag update prompts to remove this
            // .AddSequence(new CustomSequence(controller, ((sequence, o) =>
            // {
            //     StartCoroutine(GiveBook());
            //     sequence.SetStatus(true);
            // })))
            
            .AddSequence(new WaitSequence(controller, 1.0f))
            .AddSequence(new TwoToolTipSequence(controller, _adapter))

            // Scoring
            .AddSequence(new CustomSequence(controller, (sequence, o) =>
            {
                scoreStatusPanel.SetActive(true);
                sequence.SetStatus(true);
            }))
            .AddSequence(new MoveSequenceCanvas(controller, new Vector2(110, -86f), handSpeed, rectTransform))
            .AddSequence(new WaitSequence(controller, 1.0f))
            .AddSequence(new TwoToolTipSequence(controller, _adapter,
                "Ang mga bituin na ito ang iyong mga puntos","--"))
            .AddSequence(us)
            .AddSequence(new TwoToolTipSequence(controller, _adapter,
                "Kailangan mo ng isang butuin upang makapasa at magpatuloy","----"))
            .AddSequence(us)
            .AddSequence(new TwoToolTipSequence(controller, _adapter,
                "Maaring kang umulit kapag nais mong kompletuhin ang mga bituin.","--"))
            .AddSequence(us)
            .AddSequence(new TwoToolTipSequence(controller, _adapter))

            // end tutorial, go to game mode select scene
            .AddSequence(new WaitSequence(controller, 1.0f))
            .AddSequence(new CustomSequence(controller, (sequence, o) =>
            {
                endPopup.SetActive(true);
                GameManager.instance?.TutorialFinished();
                sequence.SetStatus(true);
            }));

    }

    private void Start()
    {
        GetComponent<SequenceController>().ManualStart();
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

    IEnumerator GiveBook()
    {
        var bookPos = correctBook.GetComponent<RectTransform>();
        var final = new Vector2(-250, 170);
        
        Reader.Instance.canDeduct = false;
        
        while (Vector2.Distance(bookPos.anchoredPosition, final) > 1f)
        {
            bookPos.anchoredPosition = Vector3.MoveTowards(bookPos.anchoredPosition,
                final, bookGivingSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        
        Destroy(correctBook);
    }

    private IEnumerator DragRepeat()
    {
        var rect = GetComponent<RectTransform>();
        var final = new Vector2(-250, 170);
        var original = rect.anchoredPosition;
        for (;;)
        {
            yield return new WaitForEndOfFrame();

            rect.anchoredPosition = Vector3.MoveTowards(rect.anchoredPosition, final, handSpeed * Time.deltaTime);

            if (Vector2.Distance(rect.anchoredPosition, final) < 1f)
            {
                // Reset
                rect.anchoredPosition = original;
            }
        }
    }
}