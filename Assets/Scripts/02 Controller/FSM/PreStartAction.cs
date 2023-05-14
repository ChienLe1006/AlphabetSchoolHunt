using Common.FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreStartAction : FSMAction
{
    private readonly GameManager gameManager;
    private const float timeCountDown = 3;
    private float timeCountDownLeft;

    public PreStartAction(GameManager _gameController, FSMState owner) : base(owner)
    {
        gameManager = _gameController;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        timeCountDownLeft = timeCountDown;
        gameManager.CurrentLevelManager.StartLevel();
        //gameManager.UiController.UiKillCounter.Init();
        gameManager.MainCamera.SetTargetFollow(gameManager.CurrentLevelManager.Player.transform);

        if (gameManager.CurrentGameMode == GameMode.SEEK)
        {
            gameManager.MainCamera.SetupCameraModeSeekPrestartGame();
        }
        else
        {
            gameManager.MainCamera.RunAnimationWhenPlay();
            gameManager.UiController.ActiveTutHand(true);
        }

        UltimateJoystick.EnableJoystick(Constants.MAIN_JOINSTICK);

        var seekers = GameManager.Instance.CurrentLevelManager.Seekers;
        foreach (var seeker in seekers)
        {
            if (!seeker.IsPlayer)
                seeker.SuspiciousIndicator.ShowFor(timeCountDown);
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
