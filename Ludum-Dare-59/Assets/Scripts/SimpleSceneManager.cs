using UnityEngine.SceneManagement;

public static class SimpleSceneManager
{
    private static int CurrentSceneIndex => SceneManager.GetActiveScene().buildIndex;
    private static int HighestSceneIndex => SceneManager.sceneCountInBuildSettings - 1;

    public static bool IsMainScene()
    {
        return CurrentSceneIndex == 1;
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(CurrentSceneIndex);
    }

    public static bool HasNextScene()
    {
        return CurrentSceneIndex + 1 <= HighestSceneIndex;
    }

    public static bool HasPreviousScene()
    {
        return CurrentSceneIndex > 0;
    }

    public static void LoadNextScene(bool wrap = true)
    {
        if (HasNextScene())
        {
            SceneManager.LoadScene(CurrentSceneIndex + 1);
        }
        else if (wrap)
        {
            SceneManager.LoadScene(0);
        }
    }

    public static void LoadPreviousScene(bool wrap = true)
    {
        if (HasPreviousScene())
        {
            SceneManager.LoadScene(CurrentSceneIndex - 1);
        }
        else if (wrap)
        {
            SceneManager.LoadScene(HighestSceneIndex);
        }
    }
}