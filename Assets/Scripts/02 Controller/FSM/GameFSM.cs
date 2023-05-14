using Common.FSM;

public class GameFSM : FSM
{
    public GameState CurrentGameState { get; private set; }

    private FSMState lobbyGameState;
    private LobbyAction lobbyGameAction;

    private FSMState prepareStartGameState;
    private PreStartAction preStartGameAction;


    public FSMState inGameState;
    private IngameAction inGameAction;

    private FSMState endGameState;
    private EndgameAction endGameAction;

    private FSMState showRoleState;
    private ShowRoleAction showRoleAction;

    private FSMState reviveGameState;
    private ReviveAction reviveGameAction;

    public GameFSM(GameManager gameController): base("Game FSM")
    {
        lobbyGameState = this.AddState((byte)GameState.LOBBY);
        prepareStartGameState = this.AddState((byte)GameState.PRE_START_GAME);
        inGameState = this.AddState((byte)GameState.IN_GAME);
        endGameState = this.AddState((byte)GameState.END_GAME);
        showRoleState = this.AddState((byte)GameState.SHOW_ROLE);
        reviveGameState = this.AddState((byte)GameState.REVIVE);

        lobbyGameAction = new LobbyAction(gameController, lobbyGameState);
        preStartGameAction = new PreStartAction(gameController, prepareStartGameState);
        inGameAction = new IngameAction(gameController, inGameState);
        endGameAction = new EndgameAction(gameController, endGameState);
        showRoleAction = new ShowRoleAction(gameController, showRoleState);
        reviveGameAction = new ReviveAction(gameController, reviveGameState);

        lobbyGameState.AddAction(lobbyGameAction);
        prepareStartGameState.AddAction(preStartGameAction);
        inGameState.AddAction(inGameAction);
        endGameState.AddAction(endGameAction);
        showRoleState.AddAction(showRoleAction);
        reviveGameState.AddAction(reviveGameAction);
    }

    public void ChangeState(GameState state)
    {
        CurrentGameState = state;
        switch (state)
        {
            case GameState.LOBBY:
                ChangeToState(lobbyGameState);
                break;
            case GameState.PRE_START_GAME:
                ChangeToState(prepareStartGameState);
                break;
            case GameState.IN_GAME:
                ChangeToState(inGameState);
                break;
            case GameState.END_GAME:
                ChangeToState(endGameState);
                break;
            case GameState.SHOW_ROLE:
                ChangeToState(showRoleState);
                break;
            case GameState.REVIVE:
                ChangeToState(reviveGameState);
                break;
            default:
                break;
        }
    }
}
