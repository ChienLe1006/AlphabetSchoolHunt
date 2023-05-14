using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundFxIngame : MonoBehaviour
{
    //[SerializeField] float timeDestroy;
    //TypeSoundIngame typeSound;
    //float timeCounter;
    //bool isCounting = false;

    //internal AudioSource source;

    //private void Start()
    //{
    //    source = GetComponent<AudioSource>();
    //}

    //public void Init(TypeSoundIngame type)
    //{
    //    if (source == null) source = GetComponent<AudioSource>();
    //    source.volume = SoundManager.Instance.ValueFXSound;
    //    typeSound = type;
    //    timeCounter = 0f;
    //    isCounting = true;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (isCounting)
    //    {
    //        timeCounter += Time.deltaTime;
    //        if (timeCounter >= timeDestroy)
    //        {
    //            isCounting = false;
    //            //if (GameController.Instance != null) GameController.Instance.poolController.RestoreSoundIngame(this, typeSound);
    //        }
    //    }
    //}
}
