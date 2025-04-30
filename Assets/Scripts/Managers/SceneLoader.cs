using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

internal class SceneLoader: MonoBehaviour
{

    private void Start()
    {
        if (Application.isMobilePlatform)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }
    }

    public void LoadMenuScene()
    {
        StartCoroutine(LoadAsyncScene(0));
        //SceneManager.LoadScene(0);
    }

    public void LoadGameScene()
    {
        StartCoroutine(LoadAsyncScene(1));

        //SceneManager.LoadScene(1);
    }

    public void StopGame()
    {
        Application.Quit();
    }


    IEnumerator LoadAsyncScene(int sceneBuiltIndex)
    {
        var loadedScene = SceneManager.GetActiveScene();
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneBuiltIndex, LoadSceneMode.Single);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        /*
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(loadedScene);

        // Wait until the asynchronous scene fully loads
        while (!asyncUnload.isDone)
        {
            yield return null;
        }*/
    }
}