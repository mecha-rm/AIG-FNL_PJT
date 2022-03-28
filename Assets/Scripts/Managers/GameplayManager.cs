using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// manages the gameplay
public class GameplayManager : MonoBehaviour
{
    // the player
    public UserPlayer userPlayer;

    // the computer.
    public ComputerPlayer comPlayer;

    // the race track for the game (there should only be one in the scene).
    public RaceTrack raceTrack;

    // // the audio manager for the game.
    // // audio components tied to buttons are attached to them.
    // // audio components tied to game events are done in the scripts.
    // public GameplayAudioManager audioManager;

    [Header("UI")]

    // toggle for muting the audio.
    public Toggle muteToggle;

    // lap text for the player.
    public Text userLapText;

    // text for the computer running a lap.
    public Text comLapText;

    // Start is called before the first frame update
    void Start()
    {
        // user player not set, so find them.
        if (userPlayer == null)
            userPlayer = FindObjectOfType<UserPlayer>(true);

        // computer player not set, so find them.
        if (comPlayer == null)
            comPlayer = FindObjectOfType<ComputerPlayer>(true);

        // finds the race track.
        if (raceTrack == null)
            raceTrack = FindObjectOfType<RaceTrack>();

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

    // returns to the menu.
    public void ReturnToMenu()
    {
        SceneHelper.ChangeScene("TitleScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
