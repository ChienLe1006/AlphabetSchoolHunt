using Google.Play.Common;
using Google.Play.Review;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Singleton("Popup/PopupRateGame", true)]
public class PopupRateGame : Singleton<PopupRateGame>
{
    public bool isShow { get; set; }
    [SerializeField] private Button btnConfirm;
    [SerializeField] private List<Button> listBtnStar;
    [SerializeField] private List<Sprite> listSprStar;
    [SerializeField] private List<Image> listImgStar;

    private int star;

    public override void Awake()
    {
        base.Awake();

        if (!isDestroy)
        {
            Instance.Init();
        }
        if (!isShow)
        {
            Hide();
        }

        btnConfirm.onClick.AddListener(Hide);
    }

    private void Start()
    {
        for (int i = 0; i < listBtnStar.Count; i++)
        {
            int index = i + 1;
            listBtnStar[i].onClick.AddListener(() => { OnClickStar(index); });

            listImgStar[i].sprite = listSprStar[0];
        }
        star = 0;
        // btnConfirm.interactable = false;
    }

    public void Show()
    {
        isShow = true;
        gameObject.SetActive(true);

    }

    public void Hide()
    {
        if (star <= 0)
            return;

        if (star == 5)
        {
            //Application.OpenURL(Studio1BConfig.OPEN_LINK_RATE);
            ShowReview();
        }

        if (gameObject == null)
            return;
        SoundManager.Instance.PlaySoundButton();
        gameObject.SetActive(false);
    }

    private void OnClickStar(int index)
    {
        star = index;

        for (int i = 0; i < listImgStar.Count; i++)
        {
            listImgStar[i].sprite = listSprStar[0];
        }


        for (int i = 0; i < index; i++)
        {
            listImgStar[i].sprite = listSprStar[1];
        }

        SoundManager.Instance.PlaySoundButton();
    }

    #region Review in app

    private ReviewManager _reviewManager;
    PlayAsyncOperation<PlayReviewInfo, ReviewErrorCode> playReviewInfoAsyncOperation;


    private void ShowReview(UnityAction actionErorr = null)
    {
        _reviewManager = new ReviewManager();
        playReviewInfoAsyncOperation = null;

        playReviewInfoAsyncOperation = _reviewManager.RequestReviewFlow();
        playReviewInfoAsyncOperation.Completed += playReviewInfoAsync => Complete(playReviewInfoAsync, actionErorr);
    }

    void Complete(PlayAsyncOperation<PlayReviewInfo, ReviewErrorCode> playReviewInfoAsync, UnityAction actionErorr)
    {
        if (playReviewInfoAsync.Error == ReviewErrorCode.NoError)
        {
            // display the review prompt
            var playReviewInfo = playReviewInfoAsync.GetResult();
            _reviewManager.LaunchReviewFlow(playReviewInfo);
        }
        else
        {
            if (actionErorr != null)
                actionErorr();
            // handle error when loading review prompt
            Debug.Log("Erro Review Inapp " + playReviewInfoAsync.Error);
        }
    }

    #endregion
}
