using Common.FSM;
using UnityEngine;

public class LobbyAction : FSMAction
{
    private readonly GameManager gameManager;

    public LobbyAction(GameManager _gameController, FSMState owner) : base(owner)
    {
        gameManager = _gameController;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        UltimateJoystick.DisableJoystick(Constants.MAIN_JOINSTICK);
        TutorialArrow.Instance.TurnOff();
        gameManager.UiController.ActiveTutHand(false);
        gameManager.MainCamera.SetupCameraInLobby(gameManager.CurrentLevelManager, SetupDoneCam);
      
        //if (gameManager.Profile.GetKey() >= 3)
        //{
        //    ShowChestKey();
        //}

    }

    private void SetupDoneCam()
    {
        gameManager.CurrentLevelManager.Door.Open();        
        gameManager.UiController.UiMainLobby.ActiveMainLobby();
        if (PlayerDataManager.Instance.CurrentDailyRewardDayIndex != PlayerDataManager.Instance.LastDailyRewardDayIndex && !PlayerDataManager.Instance.FirstTimePlay())
        {
            GameManager.Instance.UiController.OpenDailyReward();
        }
    }

    //private void ShowChestKey()
    //{

    //    var playerData = GameManager.Instance.PlayerDataManager;
    //    int indexReward = playerData.GetCurrentIndexRewardEndGame();
    //    if (indexReward >= playerData.DataRewardEndGame.Datas.Count)
    //    {
    //        indexReward = playerData.DataRewardEndGame.Datas.Count - 1;
    //    }

    //    var reward = playerData.DataRewardEndGame.Datas[indexReward];

    //    GameManager.Instance.UiController.OpenPopupChestKey(reward);

    //}
}