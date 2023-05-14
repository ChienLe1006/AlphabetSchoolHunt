using System;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 positionOffsetIngame;
    [SerializeField] private Vector3 positionOffsetPrestart;

    [SerializeField] private AudioListener audioListener;

    private Vector3 cameraPosition;
    private Tweener moveToPositionTween;

    private bool isFollowing;

    private Vector3 curPos, curRot;

    public void SetFollow(bool isFollow)
    {
        if (isFollow)
        {
            if (target == null)
            {
                Debug.LogError($"{nameof(isFollow)} is set to true but there is no target is set! {nameof(SetTargetFollow)} must be called first!");
            }
            isFollowing = true;
        }
        else
        {
            isFollowing = false;
        }
    }

    public void Init()
    {

    }

    public void SetTargetFollow(Transform transTarget)
    {
        target = transTarget;

    }


    public void RunAnimationWhenPlay()
    {
        cameraPosition = target.position + positionOffsetIngame;

        moveToPositionTween = transform.DOMove(cameraPosition, .5f).OnComplete(() => { SetFollow(true); }); ;
        transform.DORotate(new Vector3(50, 0, 0), .5f);

    }

    public void SetupCameraModeSeekPrestartGame()
    {
        var posCam = target.position + positionOffsetPrestart;

        transform.DOMove(posCam, 1f);
        transform.DORotate(new Vector3(66, 0, 0), 1f);
    }


    public void SetupCameraInLobby(LevelManager levelManager, Action done)
    {
        cameraPosition = Vector3.zero;
        //cameraPosition.x = levelManager.MapCenter.x;
        //cameraPosition.y = levelManager.MapSize;
        //cameraPosition.z = -levelManager.MapSize;

        moveToPositionTween = null;
        SetFollow(false);
        transform.DOMove(levelManager.CamPosition, levelManager.CamMoveDuration).SetEase(levelManager.CamEase).OnComplete(() => done?.Invoke());
        transform.DORotate(levelManager.CamRotation, levelManager.CamMoveDuration);
    }
 
    internal void ShowLevelUnlockCheck(Vector3 pos, Vector3 rot, float duration, Action callback)
    {
        isFollowing = false;
        UltimateJoystick.DisableJoystick(Constants.MAIN_JOINSTICK);
        transform.DORotate(rot, duration);
        transform.DOMove(pos, duration).OnComplete(() =>
        {
            callback?.Invoke();
            transform.DOMove(curPos, duration).SetDelay(1f).OnComplete(() =>
            {
                isFollowing = true;
                UltimateJoystick.EnableJoystick(Constants.MAIN_JOINSTICK);
            });
            transform.DORotate(curRot, duration).SetDelay(1f);
        });
    }

    internal void ShowUnlockLetter(Vector3 pos, Vector3 rot, float duration)
    {
        Vector3 curPos = transform.position;
        Vector3 curRot = transform.eulerAngles;
        this.curPos = curPos;
        this.curRot = curRot;

        isFollowing = false;
        UltimateJoystick.DisableJoystick(Constants.MAIN_JOINSTICK);
        GameManager gameManager = GameManager.Instance;
        transform.DORotate(rot, duration);
        transform.DOMove(pos, duration).OnComplete(() =>
        {
            gameManager.CurrentLevelManager.UnlockLetterUIController.LetterAppear(() =>
            {
                if (gameManager.CurrentLevelManager.NumberAlphabetUnlock == gameManager.CurrentLevelManager.Alphabets.Length)
                {
                    var reward = gameManager.PlayerDataManager.DataRewardUnlockLetter.dataRewardUnlockLetter[gameManager.CurrentLevel];
                    gameManager.UiController.OpenPopupRewardLetter(reward);
                    isFollowing = true;
                }
                else
                {
                    transform.DOMove(curPos, duration).OnComplete(() =>
                    {                        
                        UltimateJoystick.EnableJoystick(Constants.MAIN_JOINSTICK);
                        isFollowing = true;
                    });
                    transform.DORotate(curRot, duration);
                }
            });
        });
    }

    private void LateUpdate()
    {
        if (moveToPositionTween != null && moveToPositionTween.IsActive() && target != null)
        {
            Vector3 newCameraPos = target.position + positionOffsetIngame;
            if (this.cameraPosition != newCameraPos)
            {
                cameraPosition = newCameraPos;
                moveToPositionTween.ChangeStartValue(transform.position).ChangeEndValue(newCameraPos);
            }
        }

        if (isFollowing)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, target.position + positionOffsetIngame, 125 * Time.deltaTime)
                       .Set(y: positionOffsetIngame.y);
        }

        if (target != null)
        {
            audioListener.transform.position = target.position;
        }
    }
}
