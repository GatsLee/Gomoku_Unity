using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModalController : MonoBehaviour
{
    private void Awake()
    {
        if (PlayerPrefs.HasKey("isGameOver") == false)
        {
            Debug.Log("!");
            PlayerPrefs.SetInt("isGameOver", 0);
            return;
        }

        if (PlayerPrefs.GetInt("isGameOver") == 1)
        {
            SetActiveModal("GameOverModal");
            SetInactiveModal("GomokuStartModal");
        }
        else
        {
            SetInactiveModal("GameOverModal");
        }
    }
    public void SetActiveModal(string modalName)
    {
        Debug.Log("@");
        int index = 0;
        if (modalName == "GameOverModal")
        {
            index = 0;
        }
        else if (modalName == "SelectRuleModal")
        {
            index = 1;
        }
        else
        {
            index = 2;
        }
        transform.GetChild(index).gameObject.SetActive(true);
    }
    public void SetInactiveModal(string modalName)
    {
        Debug.Log("$");
        int index = 0;
        if (modalName == "GameOverModal")
        {
           index = 0;
        }
        else if (modalName == "SelectRuleModal")
        {
            index = 1;
        }
        else
        {
            index = 2;
        }
        transform.GetChild(index).gameObject.SetActive(false);
    }

    public void SetRulePlayerPrefs(string ruleName)
    {
        PlayerPrefs.SetString("ruleName", ruleName);
    }

    public void LoadPlayScene()
    {
        SceneManager.LoadScene("PlayGomokuScene");
    }

    public void ResetGameSettingInModal()
    {
        GameController.isGameOver = false;
        PlayerPrefs.SetInt("isGameOver", 0);
        GameController.isBlackTurn = true;
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
        PlayerPrefs.DeleteAll();
    }
}
