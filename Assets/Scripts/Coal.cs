using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coal : MonoBehaviour
{

    public Sprite damageSpriteCoal;
    public int hp = 4;

    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageCoal(int loss)
    {
        spriteRenderer.sprite = damageSpriteCoal;
        hp -= loss;
        if (hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }

}
