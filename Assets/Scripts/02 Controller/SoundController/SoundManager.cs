using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Singleton("SoundManager", true)]
public class SoundManager : Singleton<SoundManager>
{
    public AudioSource bgMusic;
    public AudioSource fxSound;
    public AudioSource fxSoundFootStep;
    private bool isPlayFootStep;

    #region UNITY METHOD
    private void Start()
    {
        SettingFxSound(GameManager.Instance.PlayerDataManager.GetSoundSetting());
        SettingMusic(GameManager.Instance.PlayerDataManager.GetMusicSetting());
        isPlayFootStep = false;

        PlayMusic(SoundData.Instance.AudioBg);
    }
    #endregion
    #region PRIVATE METHOD

    #endregion
    #region PUBLIC METHOD

    public bool IsOnVibration
    {
        get
        {
            return PlayerPrefs.GetInt("OnVibration", 1) == 1 ? true : false;
        }
    }

    public void SettingFxSound(bool isOn)
    {
        var vol = isOn ? 1 : 0;
        fxSound.volume = vol;
        //fxSoundFootStep.volume = vol;

        //ValueFXSound = vol;
    }
    public void SettingMusic(bool isOn)
    {
        var vol = isOn ? 0.2f : 0;
        bgMusic.volume = vol;
        //ValueBGMusic = vol;
    }
    public void PlayMusic(AudioClip clip, bool isLoop = true)
    {
        bgMusic.loop = isLoop;
        bgMusic.clip = clip;
        bgMusic.Play();
    }

    public void PlayFxSoundAt(AudioClip clip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(clip, position, fxSound.volume);
    }

    public void PlayFxSound(AudioClip clip)
    {
        fxSound.PlayOneShot(clip);
    }

    public void PlaySoundButton()
    {
        PlayFxSound(SoundData.Instance.AudioClickBtn);
    }

    public void PlaySoundSpin()
    {
        PlayFxSound(SoundData.Instance.AudioSpinWheel);
    }

    public void PlaySoundRevive()
    {
        PlayFxSound(SoundData.Instance.AudioRevive);
    }

    public void PlaySoundReward()
    {
        PlayFxSound(SoundData.Instance.AudioReward);
    }

    public void PlaySoundStartCrewmate()
    {
        PlayFxSound(SoundData.Instance.AudioStartCrewmate);
    }

    public void PlaySoundStartImpostor()
    {
        PlayFxSound(SoundData.Instance.AudioStartImpostor);
    }

    public void PlaySoundWinCrewamate()
    {
        PlayFxSound(SoundData.Instance.AudioWinCrewamate);
    }

    public void PlaySoundWinImposter()
    {
        PlayFxSound(SoundData.Instance.AudioWinImposter);
    }

    public void PlaySoundCollectible(TypeSoundIngame typeSound)
    {
        PlayFxSound(SoundData.Instance.ListAudioCollects[(int)typeSound - 1]);
    }

    public void PlaySoundDie()
    {
        PlayFxSound(SoundData.Instance.AudioDie);
    }

    public void PlaySoundUnlockNPC()
    {
        PlayFxSound(SoundData.Instance.AudioUnlockNPC);
    }

    public void PlaySoundPayCash()
    {
        PlayFxSound(SoundData.Instance.AudioPayCash);
    }

    public void PlaySoundCastNPC()
    {
        PlayFxSound(SoundData.Instance.AudioCashNPC);
    }

    public void PlaySoundCheckoutNPC()
    {
        PlayFxSound(SoundData.Instance.AudioCheckoutNPC);
    }

    public void PlaySoundBite(Vector3 position)
    {
        PlayFxSoundAt(SoundData.Instance.AudioBite, position);
        //PlayFxSound(SoundData.Instance.AudioBite);
    }

    public void PlayFootStep()
    {
        if (isPlayFootStep)
            return;

        isPlayFootStep = true;
        fxSoundFootStep.Play();

        //Analytics.LogFirstLogJoystick();
    }

    public void StopFootStep()
    {
        fxSoundFootStep.Stop();
        isPlayFootStep = false;
    }

    public void PlaySoundOverTime()
    {
        fxSound.PlayOneShot(SoundData.Instance.AudioOverTime);
    }
    #endregion
}
