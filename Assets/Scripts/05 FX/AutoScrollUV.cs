using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScrollUV : MonoBehaviour
{
	public int targetMaterialSlot;
	public Vector2 scrollSpeed;

	private Renderer rend;
	private Vector2 offset = Vector2.zero;

    // Use this for initialization
    void Start()
    {
		rend = GetComponent<Renderer>();
		offset = rend.materials[targetMaterialSlot].GetTextureOffset("_MainTex");
    }

    // Update is called once per frame
    void Update()
    {
		offset.x += Time.deltaTime * scrollSpeed.x;
		offset.y += Time.deltaTime * scrollSpeed.y;

		rend.materials[targetMaterialSlot].SetTextureOffset("_MainTex", offset);
    }
}
