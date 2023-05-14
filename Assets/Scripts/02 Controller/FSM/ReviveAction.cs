using Common.FSM;
using UnityEngine;

public class ReviveAction : FSMAction
{
    private readonly GameManager gameManager;
    private const float timeCountDown = 3;
    private float timeCountDownLeft;

    public ReviveAction(GameManager _gameController, FSMState owner) : base(owner)
    {
        gameManager = _gameController;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("Revive Action");

        RenewAllCharacter();
        UltimateJoystick.EnableJoystick(Constants.MAIN_JOINSTICK);

        timeCountDownLeft = timeCountDown;
        
        gameManager.MainCamera.SetTargetFollow(gameManager.CurrentLevelManager.Player.transform);
        if (gameManager.CurrentGameMode == GameMode.SEEK)
        {
            gameManager.MainCamera.SetFollow(false) ;
            gameManager.MainCamera.SetupCameraModeSeekPrestartGame();
        }
        else
        {
            gameManager.MainCamera.RunAnimationWhenPlay();
        }

        //gameManager.UiController.UiTimeCountdown.Setup();



    }

    private void RenewAllCharacter()
    {
        foreach (var character in GameManager.Instance.CurrentLevelManager.Characters)
        {
            if (character.IsAlive)
                character.CharacterAnimator.Play(CharacterAction.Idle.ToAnimatorHashedKey());
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        timeCountDownLeft -= Time.deltaTime;
        if (timeCountDownLeft < 0)
        {
            gameManager.GameStateController.ChangeState(GameState.IN_GAME);
        }
        UpdateCharacter();
    }

    private void UpdateCharacter()
    {
        var characters = gameManager.CurrentLevelManager.Hiders;
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i].IsAlive)
                characters[i].DoRoleAction();
        }
    }
}