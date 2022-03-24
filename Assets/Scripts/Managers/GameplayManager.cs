using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// manages the gameplay
public class GameplayManager : MonoBehaviour
{
    // // the audio manager for the game.
    // // audio components tied to buttons are attached to them.
    // // audio components tied to game events are done in the scripts.
    // public GameplayAudioManager audioManager;

    // toggle for muting the audio.
    public Toggle muteToggle;

    // Start is called before the first frame update
    void Start()
    {
        // // finds the audio manager.
        // if (audioManager == null)
        //     audioManager = FindObjectOfType<GameplayAudioManager>();

        // current setting for sound.
        if (muteToggle != null)
            muteToggle.isOn = AudioListener.pause;

    }

    // called when the mute toggle changes.
    public void OnMuteToggleChange()
    {
        AudioListener.pause = muteToggle.isOn;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}