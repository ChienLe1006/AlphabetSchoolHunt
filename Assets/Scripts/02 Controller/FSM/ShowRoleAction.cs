using Common.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRoleAction : FSMAction
{
    private readonly GameManager gameManager;
    private float time;
    private float timeShowRole = 2f;

    public ShowRoleAction(GameManager _gameController, FSMState owner) : base(owner)
    {
        gameManager = _gameController;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        time = 0;
        //gameManager.UiController.OpenUiShowRole();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        time += Time.deltaTime;
        if (time > timeShowRole)
        {
            time = 0;
            gameManager.GameStateController.ChangeState(GameState.PRE_START_GAME);
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        //gameManager.UiController.UiShowRole.OnBackPressed();
    }
}
