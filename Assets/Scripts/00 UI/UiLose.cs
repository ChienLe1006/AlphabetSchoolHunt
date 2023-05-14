using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiLose : UICanvas
{
    [SerializeField] private Button btnRetry;
    [SerializeField] private Button btnSkipLevel;

    private void Start()
    {
        btnRetry.onClick.AddListener(OnClickBtnRetry);
        btnSkipLevel.onClick.AddListener(OnClickBtnRevive);
    }

    public override void Show(bool _isShown, bool isHideMain = true)
    {
        base.Show(_isShown, isHideMain);

        if (isShow)
        {
            if (GameManager.Instance.CurrentGameMode == GameMode.HIDE)
            {
                SoundManager.Instance.PlaySoundWinImposter();
            }
            else
            {
                SoundManager.Instance.PlaySoundWinCrewamate();
            }
        }

    }

    private void OnClickBtnRetry()
    {
        OnBackPressed();

        GameManager.Instance.ShowInterAdsEndGame("end_game_lose");
        GameManager.Instance.LoadLevelScene(GameManager.Instance.CurrentLevel + 1);

        SoundManager.Instance.PlaySoundButton();
    }

    private void OnClickBtnRevive()
    {
        GetComponent<ShowRewardedController>().Show();
        SoundManager.Instance.PlaySoundButton();
    }

    public void GetRevive(bool success)
    {
        if (success)
        {
            OnReward(1);
        }
    } 

    private void OnReward(int x)
    {
        if (x <= 0 && !isShow)
            return;
        StartCoroutine(IEWaitRevive());

    }

    private IEnumerator IEWaitRevive()
    {
        yield return new WaitForSeconds(0.2f);
        GameManager.Instance.Revive();
        OnBackPressed();
    }

    public void NoInternet()
    {
        PopupDialogCanvas.Instance.Show("No Internet!");
    }

    public void ShowFail()
    {
        PopupDialogCanvas.Instance.Show("No Video!");
    }
}
