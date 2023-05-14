using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISetting : UICanvas
{
    [SerializeField] private Button btnMusic;
    [SerializeField] private Text txtMusic;
    [SerializeField] private Button btnSound;
    [SerializeField] private Text txtSound;
    [SerializeField] private Button btnRateUs;
    [SerializeField] private Button btnRestorePurchase;

    protected override void Awake()
    {
        btnMusic.onClick.AddListener(ToggleMusic);
        btnSound.onClick.AddListener(ToggleSound);
        btnRateUs.onClick.AddListener(Rate);
        btnRestorePurchase.onClick.AddListener(RestorePurchase);

        RestoreSettings();
    }

    private void RestoreSettings()
    {
        txtMusic.text = GameManager.Instance.PlayerDataManager.GetMusicSetting() ? "ON" : "OFF";
        txtSound.text = GameManager.Instance.PlayerDataManager.GetSoundSetting() ? "ON" : "OFF";
    }

    private void ToggleMusic()
    {
        bool isOn = !GameManager.Instance.PlayerDataManager.GetMusicSetting();
        txtMusic.text = isOn ? "ON" : "OFF";
        GameManager.Instance.ToggleMusic(isOn);

        SoundManager.Instance.PlaySoundButton();

        SoundManager.Instance.SettingMusic(isOn);
    }

    private void ToggleSound()
    {
        bool isOn = !GameManager.Instance.PlayerDataManager.GetSoundSetting();
        txtSound.text = isOn ? "ON" : "OFF";
        GameManager.Instance.ToggleSound(isOn);

        SoundManager.Instance.PlaySoundButton();

        SoundManager.Instance.SettingFxSound(isOn);
    }

    private void Rate()
    {
        //Application.OpenURL(Studio1BConfig.OPEN_LINK_RATE);
    }

    private void RestorePurchase()
    {
//#if UNITY_IOS
//        GameManager.Instance.IapController.RestoreButtonClick();
//#endif
    }

    public void Open()
    {
        gameObject.SetActive(true);
        GameManager.Instance.Pause();

        SoundManager.Instance.PlaySoundButton();
    }

    public void Close()
    {
        gameObject.SetActive(false);
        GameManager.Instance.Resume();

        SoundManager.Instance.PlaySoundButton();
    }
}
