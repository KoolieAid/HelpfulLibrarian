using System.Collections;
using System.Collections.Generic;
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
        if (meterFill.fillAmount <= redThreshold)
            meterFill.color = Color.Lerp(meterFill.color, redFill, 1f);
        else if ((meterFill.fillAmount <= yellowThreshold))
            meterFill.color = Color.Lerp(meterFill.color, yellowFill, 1f);
        else if ((meterFill.fillAmount <= blueThreshold))
            meterFill.color = Color.Lerp(meterFill.color, blueFill, 1f);
    }
}
