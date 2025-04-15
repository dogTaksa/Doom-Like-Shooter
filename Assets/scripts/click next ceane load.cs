using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickNextSceneLoad : MonoBehaviour
{
    public string sceneToLoad = "room ";

    void OnMouseDown()
    {
        if (FinishVerification.finishedLevel == true)
        {
            SceneManager.LoadScene(sceneToLoad);
            FinishVerification.finishedLevel = false;
        }

        else if (FinishVerification.finishedLevel == false)
        {
            Debug.Log("You have not finished this level");
        }
    }
}