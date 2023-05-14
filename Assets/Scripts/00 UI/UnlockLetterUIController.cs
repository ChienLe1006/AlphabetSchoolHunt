using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockLetterUIController : MonoBehaviour
{
    [SerializeField] private GameObject letterUnlock;
    [SerializeField] private Transform content;

    private int letterId;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        SetUp();
    }

    private void SetUp()
    {
        var level = GameManager.Instance.CurrentLevelManager;
        for (int i = 0; i < level.Alphabets.Length; i++)
        {
            GameObject letterUnlockUI = letterUnlock.Spawn(content);
            var letter = letterUnlockUI.GetComponent<LetterUnlockUI>();
            letter.Init((int)level.Alphabets[i]);

            if (PlayerDataManager.Instance.GetUnlockAlphabet((Alphabet)letter.Id))
            {
                letter.Appear();
            }
        }
    }

    internal void UnlockLetter(int id)
    {
        letterId = id;
        var level = GameManager.Instance.CurrentLevelManager;
        GameManager.Instance.MainCamera.ShowUnlockLetter(level.CamLetterPos, level.CamLetterRot, level.CamLetterDuration);       
    }

    internal void LetterAppear(Action callback)
    {
        for (int i = 0; i < content.childCount; i++)
        {
            if (content.GetChild(i).GetComponent<LetterUnlockUI>().Id == letterId)
            {
                content.GetChild(i).GetComponent<LetterUnlockUI>().Appear(() =>
                {
                    callback?.Invoke();
                });
            }
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < content.childCount; i++)
        {
            content.GetChild(i).gameObject.Despawn();
        }
    }
}
