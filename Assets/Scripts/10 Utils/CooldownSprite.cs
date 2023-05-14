using Spine.Unity.AttachmentTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer cooldownBar;
    [SerializeField] private SpriteRenderer background;

    private CameraController mainCamera;

    public SpriteRenderer Background => background;
    public SpriteRenderer CooldownBar => cooldownBar;

    private void Start()
    {
        mainCamera = GameManager.Instance.MainCamera;
    }

    public void SetValue(float percent)
    {
        CooldownBar.size = new Vector2(Mathf.Clamp(percent, 0, 1), 1);
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(mainCamera.transform.position - transform.position);
    }
}
