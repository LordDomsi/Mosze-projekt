using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    //statikus class a scene-ek bet�lt�s�hez
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
        SceneManager.LoadScene(Scene.LoadingScene.ToString()); // el�sz�r loading scene
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString()); // ha a loading scene bet�lt�tt
    }
}
