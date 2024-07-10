using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MySceneManager : MonoBehaviour
{
    private void Start()
    {
        ChangeScene("SelectScene");
    }
    public void ChangeScene(string sceneName)
    {
        StartCoroutine("LoadScene", sceneName);
    }    

    IEnumerator LoadScene(string sceneName)
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(sceneName);
    }
}
