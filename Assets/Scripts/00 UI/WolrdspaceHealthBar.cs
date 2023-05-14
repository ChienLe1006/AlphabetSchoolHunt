using UnityEngine;
using UnityEngine.UI;

public class WolrdspaceHealthBar : MonoBehaviour
{
    [SerializeField] private Character player;
    [SerializeField] private Image HealthBarImage;
    [SerializeField] private Transform HealthBarPivot;
    [SerializeField] private bool HideFullHealthBar = true;
    private PlayerDataManager playerData;
    private Camera mainCamera;

    private void Start()
    {
        playerData = PlayerDataManager.Instance;
        mainCamera = GameManager.Instance.MainCamera.GetComponent<Camera>();
    }

    void Update()
    {
        HealthBarImage.fillAmount = player.HP / player.MaxHP;

        HealthBarPivot.LookAt(mainCamera.transform.position);
        if (player.IsPlayer && playerData.CurrentAlphabetAmountInBag == playerData.BagCapacity && player.IsAlive)
        {
            player.MaxAlert.gameObject.SetActive(true);
            player.StopSkillVFX();
            player.MaxAlert.LookAt(mainCamera.transform.position);
            player.CharacterAnimator.SetBool(CharacterAction.Chase.ToAnimatorHashedKey(), false);
        }
        else
        {
            player.MaxAlert.gameObject.SetActive(false);
        }

        if (HideFullHealthBar && player.IsAlive)
        {   
            HealthBarPivot.gameObject.SetActive(HealthBarImage.fillAmount != 1);
        }
        else HealthBarPivot.gameObject.SetActive(false);
    }
}
