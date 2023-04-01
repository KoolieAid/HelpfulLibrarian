using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Image = UnityEngine.UI.Image;

public class Reader : MonoBehaviour
{
    public static Reader Instance;

    [SerializeField] private GameObject dialog;
    [SerializeField] private TextMeshProUGUI readerReq;
    [SerializeField] public Image face;
    public string requestedTitle;

    [Header("Patience Variables")]
    public float initialPatience;
    public float decrementAmount;
    private float currentPatience;
    [SerializeField] private Image patienceMeterFill;
    public UnityEvent onPatienceGone = new();
    [SerializeField] private bool isTutorial = false;
    [SerializeField] private Color highlightColor;

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

        animator.SetBool("Walking", true);

        while (true)
        {
            ShowHideRequest(canDeduct);
            var greenFill = patienceMeterFill.color;
            yield return new WaitForEndOfFrame();

            //if (readerMove.isStoped)
            if (canDeduct && !isTutorial)
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

            if (patienceMeterFill.fillAmount <= 0)
            {
                ParticleManager.Instance.PlayParticle("X");
                ParticleManager.Instance.PlayParticle("Smoke");
                Audio.Instance.PlaySfx("WrongAnswer");

                onPatienceGone.Invoke();
                yield break;
            }
        }
    }

    public void ShowHideRequest(bool b)
    {
        dialog.SetActive(b);
    }

    public void SetRequestText(BookInfo info)
    {
        readerReq.text = info.keyword.request;
        string buff = readerReq.text;
        
        foreach (var highlight in info.keyword.highlightedWords)
        {
            buff = buff.Replace(highlight, $"<color=#{ColorUtility.ToHtmlStringRGBA(highlightColor)}>{highlight}</color>");
        }

        readerReq.text = buff;
    }

    public void DeductPatience()
    {
        /// Actual computation
        /// (distance / time) * deltaTime
        /// Think of 1 second as 1 meter
        /// so we get
        /// (initialPatience / initialPatience) * delta time
        /// To simplify we get this:
        currentPatience -= Time.deltaTime;
    }

    public void ForceDeductPatience()
    {
        currentPatience -= decrementAmount;
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
