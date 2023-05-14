using Common.FSM;

public class EndgameAction : FSMAction
{
    private readonly GameManager gameManager;

    public EndgameAction(GameManager _gameController, FSMState owner) : base(owner)
    {
        gameManager = _gameController;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        gameManager.PlayerDataManager.SetAmountMoneyUnlockLevel(gameManager.CurrentLevel, gameManager.PlayerDataManager.DataMoneyUnlockLevel.moneyToUnlockLevel[gameManager.CurrentLevel]);
        UltimateJoystick.DisableJoystick(Constants.MAIN_JOINSTICK);
        SoundManager.Instance.StopFootStep();
        gameManager.UiController.ActiveTutHand(false);
        gameManager.PlayerDataManager.ClearListIdSkin();

        int gold = Constants.GOLD_WIN;
        //if (gameManager.CurrentLevelManager.Result == LevelResult.Win)
        //{
        //    gold += Constants.GOLD_WIN;
        //}

        ProcessWinLose(gold);
    }

    private void ProcessWinLose(int gold)
    {
        switch (gameManager.CurrentLevelManager.Result)
        {
            case LevelResult.Win:
                
                gameManager.UiController.OpenUiWin(gold);

                //Analytics.LogEndGameWin(GameManager.Instance.CurrentLevel, GameManager.Instance.CurrentGameMode);
                gameManager.SpawnEffWin(gameManager.CurrentLevelManager.Player.transform);
                break;
            case LevelResult.Lose:
                gameManager.UiController.OpenUiLose();

                //Analytics.LogEndGameLose(GameManager.Instance.CurrentLevel, GameManager.Instance.CurrentGameMode);
                break;
            default:
                break;
        }
    }
}
