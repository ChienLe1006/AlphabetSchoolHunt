using Common.FSM;

public class IngameAction : FSMAction
{
    private readonly GameManager gameManager;
    private PlayerDataManager playerData;
    private Character nearestCharacter;
    private NPCAlphabet nearestNPC;
    private Character player;
    private LevelManager level;

    public IngameAction(GameManager _gameController, FSMState owner) : base(owner)
    {
        gameManager = _gameController;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        playerData = gameManager.PlayerDataManager;
        player = gameManager.CurrentLevelManager.Player;
        level = gameManager.CurrentLevelManager;
        if (gameManager.CurrentGameMode == GameMode.SEEK)
        {
            gameManager.MainCamera.RunAnimationWhenPlay();
            gameManager.UiController.ActiveTutHand(true);
        }
        gameManager.UiController.UILevelInfo.SetActive(true);

        gameManager.CurrentLevelManager.Player.IntroCharacter.SetActive(false);
        gameManager.CurrentLevelManager.Player.AliveCharacter.SetActive(true);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        gameManager.CurrentLevelManager.Player.DoRoleAction();
        //gameManager.CurrentLevelManager.UpdateTimeRemainToPlay();

        TutsCheck();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        Character[] characters = gameManager.CurrentLevelManager.Characters;
        for (int i = 0; i < characters.Length; i++)
        {
            if (!characters[i].Role.HasFlag(Role.Manual))
            {
                characters[i].DoRoleAction();
            }
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        PopupCollectedAlphabet.Instance.ClearChildren();
        gameManager.UiController.UILevelInfo.SetActive(false);
        player.FieldOfView.DeleteFieldOfView();
    }

    private void TutsCheck()
    {
        if (!playerData.FirstAlphabetUnlock)
        {
            nearestNPC = gameManager.CurrentLevelManager.FindNearestNPC(gameManager.CurrentLevelManager.Player.transform.position);
            TutorialArrow.Instance.InitArrow(player.transform.position, nearestNPC.UnlockTrans.position);
        }
        else if (playerData.FirstAlphabetUnlock && !playerData.FirstAlphabetKill)
        {
            nearestCharacter = gameManager.CurrentLevelManager.FindNearestCharacter(playerData.FirstUnlockAlphabet, gameManager.CurrentLevelManager.Player.transform.position);
            TutorialArrow.Instance.InitArrow(player.transform.position, nearestCharacter.transform.position);
        }
        else if (playerData.FirstAlphabetUnlock && playerData.FirstAlphabetKill && !playerData.FirstAlphabetReturn && playerData.CurrentAlphabetAmountInBag == playerData.BagCapacity && playerData.FirstNPCAlphabet != null)
        {
            TutorialArrow.Instance.InitArrow(player.transform.position, playerData.FirstNPCAlphabet.ReturnPlace.position);
        }
        else if (level.NumberAlphabetUnlock == 2 && !playerData.FirstGoToUpgrade && level.UpgradeDesk != null)
        {
            TutorialArrow.Instance.InitArrow(player.transform.position, level.UpgradeDesk.position);
        }
        else if (playerData.GetGold() >= playerData.GetAmountMoneyUnlockLevel(gameManager.CurrentLevel) && level.CheckUnlockLevel.activeInHierarchy)
        {
            TutorialArrow.Instance.InitArrow(player.transform.position, level.CheckUnlockLevel.transform.position);
        }        
        else TutorialArrow.Instance.TurnOff();
    }
}
