using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiRemainTimeToPlay : MonoBehaviour
{
    [SerializeField] private Text txtTime;
    [SerializeField] private Image imgTimeRunner;

    private float timeRemain;
    public void ActiveUI(bool isActive)
    {
        this.gameObject.SetActive(isActive);
        txtTime.color = Color.white;
    }

    //public void SetLayout(float time)
    //{
    //    timeRemain = time > 0 ? time : 0;

    //    imgTimeRunner.fillAmount = 1 - timeRemain / GameManager.Instance.CurrentLevelManager.TimeToPlay;

    //    TimeSpan timeSpan = TimeSpan.FromSeconds(Mathf.RoundToInt(timeRemain));
    //    string newTime = Helper.FormatTime(timeSpan.Minutes, timeSpan.Seconds);

    //    if (timeRemain < 5 && txtTime.text != newTime)
    //    {
    //        RunFxOverTime();
    //    }

    //    txtTime.text = newTime;
    //}

    //private void RunFxOverTime()
    //{
    //    txtTime.color = Color.red;
    //    txtTime.transform.DOPunchScale(Vector3.one * 1.2f, 0.2f, 5);
    //    SoundManager.Instance.PlaySoundOverTime();
    //}
}
