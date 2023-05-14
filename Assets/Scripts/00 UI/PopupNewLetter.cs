using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupNewLetter : UICanvas
{
    [SerializeField] private Transform content;
    [SerializeField] private Text price, letter;
    [SerializeField] private Transform alphabet;
    private int id;

    internal void Init(int idAlphabet)
    {
        UltimateJoystick.DisableJoystick(Constants.MAIN_JOINSTICK);
        content.DOScale(1, 0.4f).SetEase(Ease.OutBack);
        id = idAlphabet;

        price.text = $"{PlayerDataManager.Instance.DataMoneyReceiveFromAlphabet.dataMoneyReceiveFromAlphabet[(Alphabet)idAlphabet]}";
        letter.text = $"{(Alphabet)idAlphabet}";
;
        for (int i = 0; i < alphabet.childCount; i++)
        {
            alphabet.GetChild(i).gameObject.SetActive(false);
        }
        alphabet.GetChild(idAlphabet).gameObject.SetActive(true);
    }

    public override void OnBackPressed()
    {
        base.OnBackPressed();
        SoundManager.Instance.PlaySoundButton();

        GameManager.Instance.CurrentLevelManager.UnlockLetterUIController.UnlockLetter(id);
    }

    private void OnDisable()
    {
        UltimateJoystick.EnableJoystick(Constants.MAIN_JOINSTICK);
        content.localScale = Vector3.zero;
        content.DOKill();
    }
}
