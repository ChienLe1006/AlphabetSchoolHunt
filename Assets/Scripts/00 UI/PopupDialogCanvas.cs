using UnityEngine;
using UnityEngine.UI;

[Singleton("Popup/PopupDialogCanvas", true)]
public class PopupDialogCanvas : Singleton<PopupDialogCanvas>
{
    public bool isShow { get; set; }
    [SerializeField]
    private Text text;
    [SerializeField] private Button btnClose;
    public override void Awake()
    {
        base.Awake();

        if (!isDestroy)
        {
            Instance.Init();
        }
        if (!isShow)
        {
            Hide();
        }

        btnClose.onClick.AddListener(Hide);
    }

    public void Show(string txt)
    {
        isShow = true;
        text.text = txt;
        gameObject.SetActive(true);

    }

    public void Hide()
    {

        if (gameObject == null)
            return;
        gameObject.SetActive(false);
        isShow = false;

        SoundManager.Instance.PlaySoundButton();
    }

}
