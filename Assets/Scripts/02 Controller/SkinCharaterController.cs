using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinCharaterController : MonoBehaviour
{
    [SerializeField] private DataTextureSkin dataTextureSkin;
    [SerializeField] private SkinnedMeshRenderer skinnedBodyMeshRenderer;
    [SerializeField] protected SkinnedMeshRenderer introMeshRenderer;
    [SerializeField] private SpriteRenderer elementLogo;
    [SerializeField] private List<GameObject> alphabetObjs;

    public DataTextureSkin DataTextureSkin => dataTextureSkin;

    public SkinnedMeshRenderer SkinnedBodyMeshRenderer => skinnedBodyMeshRenderer;

    public MeshRenderer MeshHat { get; set; }


    public void Init()
    {
        PlayerDataManager playerData = PlayerDataManager.Instance;
        ChangeSkinCharacter(playerData.GetIdEquipSkin(TypeEquipment.SKIN));
        
        if (playerData.FirstSkillUnlock) ChangeElementLogo(playerData.GetIdEquipElement());
    }

    public void ChangeSkin(TypeEquipment typeEquipment, int id)   // old method
    {
        switch (typeEquipment)
        {
            case TypeEquipment.HAT:
                {
                    //ChangeHat(id);
                }
                break;
            case TypeEquipment.SKIN:
                {
                    ChangeSkinCharacter(id);
                }
                break;
        }
    }

    public void InitAlphabet(int id)
    {
        for (int i = 0; i < alphabetObjs.Count; i++)
        {
            alphabetObjs[i].SetActive(false);
        }
        alphabetObjs[id].SetActive(true);
    }

    internal void ChangeElementLogo(int id)
    {
        elementLogo.sprite = PlayerDataManager.Instance.DataTexture.GetIconElement(id);
    }

    internal void ChangeSkinCharacter(int id)
    {
        var mat = PlayerDataManager.Instance.DataSkinMaterial.skinMaterials[(SkinCharacter)id];
        skinnedBodyMeshRenderer.sharedMaterial = mat;
        if (introMeshRenderer) introMeshRenderer.sharedMaterial = mat;
    }
}
