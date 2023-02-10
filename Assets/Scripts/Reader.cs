using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Image = UnityEngine.UI.Image;

public class Reader : MonoBehaviour
{
    [SerializeField] private GameObject dialog;
    [SerializeField] private Image imageComp;
    [SerializeField] public Image face;
    public string requestedTitle;

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

    private IEnumerator Start()
    {
        currentPatience = initialPatience;

        while (true)
        {
            var greenFill = patienceMeterFill.color;
            yield return new WaitForSeconds(1.0f);
            currentPatience -= incrementAmount;
            patienceMeterFill.fillAmount = currentPatience / initialPatience;
            
            var bg = redFill;

            bg = Color.Lerp(bg, yellowFill, System.Convert.ToSingle(patienceMeterFill.fillAmount >= redThreshold));
            bg = Color.Lerp(bg, blueFill, System.Convert.ToSingle(patienceMeterFill.fillAmount >= yellowThreshold));
            bg = Color.Lerp(bg, greenFill, System.Convert.ToSingle(patienceMeterFill.fillAmount >= blueThreshold));

            patienceMeterFill.color = bg;
            
            if (currentPatience <= 0)
            {
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
        Debug.LogWarning("Patience Deducted");
        currentPatience -= incrementAmount;
    }
}
