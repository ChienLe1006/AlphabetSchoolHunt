using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiShowRole : UICanvas
{
    [SerializeField] private List<GameObject> listObjCrew;
    [SerializeField] private List<GameObject> listObjImposter;
    [SerializeField] private SkinCharaterController skin;

    public void Init()
    {
        for (int i = 0; i < listObjCrew.Count; i++)
        {
            listObjCrew[i].SetActive(false);
            listObjImposter[i].SetActive(false);
        }

        if (GameManager.Instance.CurrentGameMode == GameMode.HIDE)
        {
            for (int i = 0; i < listObjCrew.Count; i++)
            {
                listObjCrew[i].SetActive(true);

            }
        }
        else
        {
            for (int i = 0; i < listObjCrew.Count; i++)
            {

                listObjImposter[i].SetActive(true);
            }
        }

        skin.Init();
        skin.ChangeElementLogo(PlayerDataManager.Instance.GetIdEquipElement());

        if (GameManager.Instance.CurrentGameMode == GameMode.HIDE)
        {
            SoundManager.Instance.PlaySoundStartCrewmate();
        }
        else
        {
            SoundManager.Instance.PlaySoundStartImpostor();
        }
    }

    public override void Show(bool _isShown, bool isHideMain = true)
    {
        base.Show(_isShown, isHideMain);
        if (IsShow)
            Init();
    }
}
