using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Quản lý load scene và game state
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Title("Components")]
    [SerializeField] private UiController uiController;
    [SerializeField] private CameraController mainCamera;
    [SerializeField] private PlayerDataManager playerDataManager;
    //[SerializeField] private IapController iap;

    [Title("Game Rule")]
    [SerializeField] private int loopLevelMin = 5;
    [SerializeField] private List<int> loopExceptionLevels;

    [Title("Prefabs")]
    [SerializeField] private Character[] characterPrefabs;
    [SerializeField] private GameObject fxWinPrefab;
    private GameObject objFxWin;

    private LevelManager currentLevelManager;
    private CharactersContainer characterPool;

    public event Action<LevelResult> OnLevelEnd;
    public event Action OnRevive;

    public int CurrentLevel { get; private set; }
    public GameFSM GameStateController { get; set; }
    public PlayerDataManager PlayerDataManager => playerDataManager;
    public CameraController MainCamera => mainCamera;
    public UiController UiController => uiController;
    public GameMode CurrentGameMode { get; private set; }
    public CharactersContainer CharacterPool => characterPool;
    public Character[] CharacterPrefabs => characterPrefabs;
    public LevelManager CurrentLevelManager
    {
        get
        {
            if (!currentLevelManager)
            {
                throw new System.EntryPointNotFoundException($"Level {CurrentLevel} does not have a LevelManager or Level has not been loaded!");
            }
            return currentLevelManager;
        }
        private set => currentLevelManager = value;
    }
    //public IapController IapController { get => iap; }
    public Profile Profile { get; private set; }

    private void Awake() {
        Screen.SetResolution((int)(Screen.width / 1.5f), (int)(Screen.height / 1.5f), true);
        Application.targetFrameRate = 60;
        Instance = this;
        characterPool = new CharactersContainer(characterPrefabs[0]);
        GameStateController = new GameFSM(this);
        Profile = new Profile();

        if (playerDataManager.NewUser())
        {
            NewUserGift();
            playerDataManager.SetNewUser(false);
        }
    }

    private void Start()
    {
        int currentLvl = PlayerDataManager.GetDataLevel().Level;
        int sceneIndex = currentLvl + 1;
        LoadLevelScene(sceneIndex);

        UiController.Init();
        MainCamera.Init();
    }

    private void NewUserGift()
    {
        playerDataManager.SetUnlockSkin(TypeEquipment.SKIN, 0);
    }

    /// <summary>
    /// Load level bằng sceneBuildIndex
    /// </summary>
    /// <param name="sceneIndex">sceneBuildIndex</param>
    public void LoadLevelScene(int sceneIndex)
    {
        if (CurrentLevel != 0 && SceneManager.sceneCount != 1)
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));

        CurrentLevel = sceneIndex - 1;
        var dataLevel = PlayerDataManager.GetDataLevel();
        dataLevel.Level = sceneIndex - 1;
        PlayerDataManager.SetDataLevel(dataLevel);
        //CurrentLevelManager = null;

        var asyncOperation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

        StartCoroutine(WaitForSceneLoaded(asyncOperation));
    }

    private IEnumerator WaitForSceneLoaded(AsyncOperation asyncOperation)
    {
        uiController.OpenLoading(true);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        uiController.OpenLoading(false);
    }

    /// <summary>
    /// LevelManager ở mỗi scene khi được load sẽ gọi hàm này.
    /// </summary>
    /// <param name="levelManager"></param>
    public void SetUpNewLevel(LevelManager levelManager)
    {
        CurrentLevelManager = levelManager;
        GameStateController.ChangeState(GameState.LOBBY);
    }

    public void StartCurrentLevel()
    {
        //Analytics.LogTapToPlay();

        this.CurrentGameMode = GetRandomGameMode();
        GameStateController.ChangeState(GameState.PRE_START_GAME);
    }

    private GameMode GetRandomGameMode()
    {
        return CurrentLevelManager.IsSpecialLevel ? GameMode.HIDE : GameMode.SEEK; //(GameMode)UnityEngine.Random.Range(1, 3);
    }

    public void EndCurrentLevel()
    {
        StartCoroutine(DelayedEndgame());
    }

    private IEnumerator DelayedEndgame()
    {
        do
        {
            bool hasSeekerAttacking = false;

            foreach (var seeker in CurrentLevelManager.Seekers)
            {
                if (seeker.IsAttacking)
                {
                    hasSeekerAttacking = seeker.IsAttacking;
                    break;
                }
            }

            if (hasSeekerAttacking)
            {
                yield return null;
            }
            else
            {
                break;
            }

        } while (true);

        GameStateController.ChangeState(GameState.END_GAME);
        OnLevelEnd?.Invoke(CurrentLevelManager.Result);

        if (CurrentLevelManager.Result == LevelResult.Win)
        {
            CurrentLevel = GetNextLevel();
            var levelData = PlayerDataManager.GetDataLevel();
            levelData.Level = CurrentLevel;
            levelData.DisplayLevel++;
            levelData.IsKeyCollected = false;
            PlayerDataManager.SetDataLevel(levelData);
        }
    }

    private int GetNextLevel()
    {
        if (PlayerDataManager.GetMaxLevelReached() + 2 < SceneManager.sceneCountInBuildSettings)
        {
            return CurrentLevel + 1;
        }

        // Begin loop
        int nextLevel = loopLevelMin;
        for (int i = 0; i < 10; i++)
        {
            nextLevel = UnityEngine.Random.Range(loopLevelMin, SceneManager.sceneCountInBuildSettings);
            if (!loopExceptionLevels.Contains(nextLevel))
            {
                return nextLevel;
            }
        }

        return nextLevel + 1;
    }

    public void Revive()
    {
        CurrentLevelManager.Revive();
        GameStateController.ChangeState(GameState.REVIVE);
        OnRevive?.Invoke();

        SoundManager.Instance.PlaySoundRevive();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        UltimateJoystick.DisableJoystick(Constants.MAIN_JOINSTICK);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        if (GameStateController.currentState.GetId() != (int)GameState.LOBBY
            && GameStateController.currentState.GetId() != (int)GameState.END_GAME)
        {
            UltimateJoystick.EnableJoystick(Constants.MAIN_JOINSTICK);
        }
    }

    public void ToggleMusic(bool isOn)
    {
        //TODO
        if (isOn)
        {
            PlayerDataManager.SetMusicSetting(true);
        }
        else
        {
            PlayerDataManager.SetMusicSetting(false);
        }
    }

    public void ToggleSound(bool isOn)
    {
        //TODO
        if (isOn)
        {
            PlayerDataManager.SetSoundSetting(true);
        }
        else
        {
            PlayerDataManager.SetSoundSetting(false);
        }
    }

    public void OnKeyCollected()
    {
        var dataLevel = PlayerDataManager.GetDataLevel();
        dataLevel.IsKeyCollected = true;
        PlayerDataManager.SetDataLevel(dataLevel);
    }

    private void Update()
    {
        GameStateController.Update();
    }

    private void FixedUpdate()
    {
        GameStateController.FixedUpdate();
    }

    //private void OnApplicationQuit()
    //{
    //    playerDataManager.SetQuitGame(true);
    //    playerDataManager.SetQuitTime(DateTime.Now.ToString());
    //    Debug.Log(DateTime.Now.ToString());
    //}

    #region Ads
    public void LoadBannerAds()
    {
        if (playerDataManager.IsNoAds())
            return;

        //AdManager.Instance.ShowBanner();
    }

    public void ShowInterAds(string _placement)
    {
        GetComponent<ShowInterstitialController>().Show();
    }

    public void ShowInterAdsEndGame(string _placement)
    {
        if (playerDataManager.IsNoAds())
            return;

        int lvlShowAds = 0; 

        if (CurrentLevel <= lvlShowAds)
            return;

        int numberPlay = Profile.GetNumberPlay();

        ShowInterAds(_placement);
        numberPlay++;
        Profile.SetNumberPlay(numberPlay);
    }
    #endregion

    public void SpawnEffWin(Transform transPlayer)
    {

        var pos = transPlayer.position;
        pos.z += 8;
        pos.y += 8;
        if (objFxWin == null)
        {
            Instantiate(fxWinPrefab, pos, Quaternion.identity);
        }
        else
        {
            objFxWin.SetActive(false);
            objFxWin.transform.position = pos;
            objFxWin.SetActive(true);
        }

    }
}
