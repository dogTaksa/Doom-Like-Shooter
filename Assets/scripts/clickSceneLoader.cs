using UnityEngine;
using UnityEngine.SceneManagement;

public class clickSceneLoader : MonoBehaviour
{
    public string sceneToLoad = "Level1";

    private void OnMouseDown()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
