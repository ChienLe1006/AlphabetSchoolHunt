using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Singleton("SoundManager", true)]
public class SoundData : Singleton<SoundData>
{
    public AudioClip AudioClickBtn;

    public AudioClip AudioFootStep;
    public AudioClip AudioRevive;
    public AudioClip AudioReward;
    public AudioClip AudioSpinWheel;
    public AudioClip AudioStartCrewmate;
    public AudioClip AudioStartImpostor;
    public AudioClip AudioWinCrewamate;
    public AudioClip AudioWinImposter;
    public AudioClip AudioDie;
    public AudioClip AudioUnlockNPC;
    public AudioClip AudioPayCash;
    public AudioClip AudioCashNPC;
    public AudioClip AudioCheckoutNPC;

    public AudioClip AudioBg;
    public AudioClip AudioBite;

    public AudioClip AudioOverTime;


    public List<AudioClip> ListAudioCollects;
}
