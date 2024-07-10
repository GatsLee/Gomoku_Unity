using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private Color mGreyColor = new Color(Color.grey.r, Color.grey.g, Color.grey.b, 1.0f);
    private Color mOriginalColor;

    // Start is called before the first frame update
    void Start()
    {
        mOriginalColor = transform.GetChild(0).GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
        Image blackUI = transform.GetChild(0).GetComponent<Image>();
        Image whiteUI = transform.GetChild(1).GetComponent<Image>();
        if (GameController.isBlackTurn == true)
        {
            blackUI.color = mOriginalColor;
            whiteUI.color = mGreyColor;
        }
        else
        {
            blackUI.color = mGreyColor;
            whiteUI.color = mOriginalColor;
        }
    }
}
