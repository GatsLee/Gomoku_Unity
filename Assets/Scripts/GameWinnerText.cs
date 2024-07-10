using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameWinnerText : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        string winner = PlayerPrefs.GetString("winner");
        if (text.text == "")
        {
            if (winner == "Black")
            {
                text.text = "Black Wins!";
            }
            else if (winner == "White")
            {
                text.text = "White Wins!";
            }
            else
            {
                text.text = "Draw!";
            }
        }
        else
        {
            Debug.Log("Text already set");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
