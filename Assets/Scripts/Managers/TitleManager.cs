using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// manages the title screen.
public class TitleManager : MonoBehaviour
{
    // the audio manager for the game.
    // audio components tied to buttons are attached to them.
    // audio components tied to game events are done in the scripts.
    // public TitleAudioManager audioManager;

    // the volume slider.
    public Slider volumeSlider;

    // toggle for muting the audio.
    public Toggle muteToggle;

    // screen size selector
    public Dropdown screenSizeSel;

    // Start is called before the first frame update
    void Start()
    {
        // // finds the audio manager.
        // if (audioManager == null)
        //     audioManager = FindObjectOfType<TitleAudioManager>();

        // grabs the volume slider. There should only be one 
        if (volumeSlider == null)
            volumeSlider = FindObjectOfType<Slider>(true);

        // adjusts the audio levels.
        if (volumeSlider != null)
            volumeSlider.value = AudioListener.volume;

        // should only be one toggle.
        if (muteToggle == null)
            muteToggle = FindObjectOfType<Toggle>();

        // current setting.
        if (muteToggle != null)
            muteToggle.isOn = AudioListener.pause;

        // only one drop down.
        if(screenSizeSel == null)
            screenSizeSel = FindObjectOfType<Dropdown>();

        // sets to current screen size.
        if (screenSizeSel != null)
        {
            // if in full-screen, use option 0.
            if (Screen.fullScreen)
            {
                screenSizeSel.value = 0;
            }
            else // specifics screen size.
            {
                // checks current screen size.
                int screenY = Screen.height;

                // checks screen size to see default value.
                switch (screenY)
                {
                    case 1080: // big
                        screenSizeSel.value = 1;
                        break;

                    case 720: // medium
                        screenSizeSel.value = 2;
                        break;

                    case 480: // small
                        screenSizeSel.value = 3;
                        break;
                }
            }

        }
    }

    // called when the volume slider changes.
    public void OnVolumeSliderChange()
    {
        AudioListener.volume = Mathf.Clamp01(volumeSlider.value);
    }

    // called when the mute toggle changes.
    public void OnMuteToggleChange()
    {
        AudioListener.pause = muteToggle.isOn;
    }

    // called by dropdown to change screen size.
    public void OnScreenSizeDropdownChange()
    {
        // get screen size from dropdown
        if (screenSizeSel != null)
            ChangeScreenSize(screenSizeSel.value);
    }

    // called when the screen size changes.
    public void ChangeScreenSize(int option)
    {
        switch (option)
        {
            case 0: // Full Screen
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Screen.fullScreen = true;
                break;

            case 1: // 1920 X 1080
                Screen.SetResolution(1920, 1080, FullScreenMode.MaximizedWindow);
                Screen.fullScreen = false;
                break;

            case 2: // 1280 X 720
                Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
                Screen.fullScreen = false;
                break;

            case 3: // 854 X 480 (854 rounded up from 853.333)
                Screen.SetResolution(854, 480, FullScreenMode.Windowed);
                Screen.fullScreen = false;
                break;
        }
    }

    // starts the game.
    public void StartGame()
    {
        SceneHelper.ChangeScene("GameScene");
    }

    // quits the game.
    public void QuitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
