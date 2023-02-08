using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PatienceColorChanger : MonoBehaviour
{
    public Image meterFill;
    public Color blueFill;
    public Color yellowFill;
    public Color redFill;

    public float blueThreshold;
    public float yellowThreshold;
    public float redThreshold;

    // Update is called once per frame
    void Update()
    {
        var bg = redFill;

        bg = Color.Lerp(bg, yellowFill, System.Convert.ToSingle(meterFill.fillAmount >= redThreshold));
        bg = Color.Lerp(bg, blueFill, System.Convert.ToSingle(meterFill.fillAmount >= yellowThreshold));

        meterFill.color = bg;
    }
}
