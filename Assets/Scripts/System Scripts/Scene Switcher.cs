using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // This method loads the next scene in build index order
    public void SwitchToNextScene()
    {
        Debug.Log("Button clicked - attempting scene switch");

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        Debug.Log($"Current scene index: {currentSceneIndex}, trying to load: {nextSceneIndex}");

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("Scene exists - loading...");
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No next scene in Build Settings!");
        }
    }


}
