using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    //statikus class a scene-ek betöltéséhez
    public enum Scene
    {
        MenuScene,
        GameScene,
        CutScene,
        LoadingScene
    }

    private static Scene targetScene;

    public static void LoadScene(Scene targetScene)
    {
        Loader.targetScene = targetScene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString()); // elõször loading scene
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString()); // ha a loading scene betöltött
    }
}
