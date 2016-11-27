using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    const string masterVolumeParam = "MasterVolume";
    const string sfxVolumeParam = "SfxVolume";
    const string musicVolumeParam = "MusicVolume";

    const float muteVolume = -80f;
    const float fullVolume = 0;

    public AudioMixer mixer;

    void Start()
    {
        mixer.SetFloat(musicVolumeParam, PersistentData.MusicDisabled ? muteVolume : fullVolume);
        mixer.SetFloat(sfxVolumeParam, PersistentData.SfxDisabled ? muteVolume : fullVolume);

        PostOffice.musicToggled += OnMusicToggled;
        PostOffice.sfxToggled += OnSfxToggled;
    }

    void OnDestroy()
    {
        PostOffice.musicToggled -= OnMusicToggled;
        PostOffice.sfxToggled -= OnSfxToggled;
    }

    void OnMusicToggled(bool isEnabled)
    {
        mixer.SetFloat(musicVolumeParam, PersistentData.MusicDisabled ? muteVolume : fullVolume);
    }

    void OnSfxToggled(bool isEnabled)
    {
        mixer.SetFloat(sfxVolumeParam, PersistentData.SfxDisabled ? muteVolume : fullVolume);
    }
}
