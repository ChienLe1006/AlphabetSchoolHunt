using UnityEngine;

public class TutorialArrow : Singleton<TutorialArrow> 
{
    [SerializeField] private LineRenderer arrow;
    [SerializeField] private Material arrowMat;
    private Vector3 pos1, pos2;
    private float x;

    private void Update()
    {
        InitArrow(pos1, pos2);
        x -= Time.deltaTime * 2f;
        arrowMat.mainTextureOffset = new Vector2(x, 0);
    }

    internal void InitArrow(Vector3 pos1, Vector3 pos2)
    {
        this.pos1 = pos1;
        this.pos2 = pos2;
        arrow.SetPosition(0, this.pos1);
        arrow.SetPosition(1, this.pos2);

        gameObject.SetActive(true);
    }

    internal void TurnOff()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (arrowMat != null)
        {
            arrowMat.mainTextureOffset = Vector2.zero;
        }
    }
}
