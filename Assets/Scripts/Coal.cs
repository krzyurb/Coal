using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coal : MonoBehaviour
{

    public Sprite dmgSprite;
    public int hp = 40;

    private SpriteRenderer spriteRenderer;



    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageCoal(int loss)
    {
        hp -= loss;
        if (hp == 0)
        {
            gameObject.SetActive(false);
        }
        if(hp == 1)
        {
           // todo set broke coal sprinte spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Graphics_3"); ;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
