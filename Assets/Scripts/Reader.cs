using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Events;
using Image = UnityEngine.UI.Image;

public class Reader : MonoBehaviour
{
    public static Reader Instance;

    [SerializeField] private GameObject dialog;
    [SerializeField] private Image imageComp;
    [SerializeField] public Image face;
    public string requestedTitle;

    [Header("Patience Variables")]
    public float initialPatience;
    public float incrementAmount;
    private float currentPatience;
    [SerializeField] private Image patienceMeterFill;
    public UnityEvent onPatienceGone = new();

    [Header("Patience Bar Aesthetics")]
    [SerializeField] private Color blueFill;
    [SerializeField] private Color yellowFill;
    [SerializeField] private Color redFill;

    [SerializeField] [Range(0, 1)] private float blueThreshold;
    [SerializeField] [Range(0, 1)] private float yellowThreshold;
    [SerializeField] [Range(0, 1)] private float redThreshold;

    public bool canDeduct;

    private ReaderMove readerMove;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator Start()
    {
        readerMove = GetComponent<ReaderMove>();
        currentPatience = initialPatience;

        //animator.runtimeAnimatorController = 
        animator.SetBool("Walking", true);

        while (true)
        {
            ShowHideRequest(canDeduct);
            var greenFill = patienceMeterFill.color;
            yield return new WaitForSeconds(1.0f);

            //if (readerMove.isStoped)
            if (canDeduct)
            {
                animator.SetBool("Walking", false);
                DeductPatience();
            }

            patienceMeterFill.fillAmount = currentPatience / initialPatience;
            animator.SetFloat("Patience", patienceMeterFill.fillAmount);

            var bg = redFill;

            bg = Color.Lerp(bg, yellowFill, System.Convert.ToSingle(patienceMeterFill.fillAmount >= redThreshold));
            bg = Color.Lerp(bg, blueFill, System.Convert.ToSingle(patienceMeterFill.fillAmount >= yellowThreshold));
            bg = Color.Lerp(bg, greenFill, System.Convert.ToSingle(patienceMeterFill.fillAmount >= blueThreshold));

            patienceMeterFill.color = bg;

            if (currentPatience <= 0)
            {
                ReaderManager.Instance.particles["X"].Play();
                ReaderManager.Instance.particles["Smoke"].Play();

                onPatienceGone.Invoke();
                yield break;
            }
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

    public void DeductPatience()
    {
        currentPatience -= incrementAmount;
    }

    public float GetPatience()
    {
        return patienceMeterFill.fillAmount;
    }

    public void TriggerWrongBookAnimation()
    {
        animator.SetTrigger("Wrong Book");
    }

}
