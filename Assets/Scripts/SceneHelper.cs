using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// scene assist script, which was imported from an existing project of mine.
public class SceneHelper : MonoBehaviour
{
    // changes the scene using the scene number.
    public static void ChangeScene(int scene)
    {
        SceneManager.LoadScene(scene);

    }

    // changes the scene using the scene name.
    public static void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    // returns the skybox of the scene.
    public static Material GetSkybox()
    {
        return RenderSettings.skybox;
    }

    // sets the skybox of the scene.
    public static void SetSkybox(Material newSkybox)
    {
        RenderSettings.skybox = newSkybox;
    }

    // returns 'true' if the game is full screen.
    public static bool IsFullScreen()
    {
        return Screen.fullScreen;
    }

    // sets 'full screen' mode
    public static void SetFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }

    // toggles the full screen.
    public static void FullScreenToggle()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    // called to change the screen size.
    public static void ChangeScreenSize(int width, int height, FullScreenMode mode)
    {
        Screen.SetResolution(width, height, mode);
    }

    // called to change the screen size.
    public static void ChangeScreenSize(int width, int height, FullScreenMode mode, bool fullScreen)
    {
        ChangeScreenSize(width, height, mode);
        Screen.fullScreen = fullScreen;
    }

    // exits the game
    public static void ExitApplication()
    {
        Application.Quit();
    }
}
