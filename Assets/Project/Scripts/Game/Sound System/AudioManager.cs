using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : Singleton<AudioManager>, IDataPersistence
{
    private string musicString;
    //public bool soundOff;

    #region Load and Save

    public void LoadData(GameData data)
    {
        MusicVolume(data._Value_Music);
        SfxVolume(data._Value_Sfx);
    }

    public void SaveData(ref GameData data)
    {

    }

    #endregion

    [SerializeField] public Sound[] musicSounds, sfxSounds;
    [Header("Sound Source")]
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource sfxSource;

    private void Start()
    {
        DataPersistenceManager.Ins.CallData(this);
    }

    #region Sound Choose

    public void PlayMusic(string name)
    {
        //Search thought the list the sound name
        Sound chosseSound = Array.Find(musicSounds, x => x.soundName == name);

        //If find it, play the music.Else debug
        if(chosseSound == null)
        {
            musicString = null;
            musicSource.Stop();
        }
        else
        {
            musicString = name;
            musicSource.clip = chosseSound.soundClip;
            musicSource.Play();
        }
    }

    public void PlaySfx(string name)
    {
        //Same as above
        Sound chosseSound = Array.Find(sfxSounds, x => x.soundName == name);

        if (chosseSound == null)
        {
            sfxSource.Stop();
        }
        else
        {
            sfxSource.clip = chosseSound.soundClip;
            sfxSource.Play();
        }
    }

    public void ResetSound()
    {
        PlayMusic("");
        PlaySfx("");
    }

    #endregion

    #region Sound Slide

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SfxVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    #endregion

    #region Sound Control
    public void MusicControl()
    {
        if (GameManager.Ins.IsState(GameState.MainMenu) || GameManager.Ins.IsState(GameState.WeaponShop) || GameManager.Ins.IsState(GameState.SkinShop))
        {
            if (musicString == Constants.MENU_THEME)
            {
                return;
            }
            else
            {
                PlayMusic(Constants.MENU_THEME);
            }
        }
        else if (GameManager.Ins.IsState(GameState.Gameplay) || GameManager.Ins.IsState(GameState.Setting))
        {
            if (musicString == Constants.BATTLE_THEME)
            {
                return;
            }
            else
            {
                PlayMusic(Constants.BATTLE_THEME);
            }
        }
        else if (GameManager.Ins.IsState(GameState.Revive))
        {
            ResetSound();
        }
        else if (GameManager.Ins.IsState(GameState.Win))
        {
            ResetSound();
        }
        else if (GameManager.Ins.IsState(GameState.Lose))
        {
            ResetSound();
        }
        else
        {
            ResetSound();
        }
    }

    #endregion

}
