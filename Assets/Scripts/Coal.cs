using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coal : MonoBehaviour {

    public Sprite damageSpriteCoal;
    public int hp = 4;

    private SpriteRenderer spriteRenderer;

	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
    }

	public void DamageCoal (int loss, Player p) {
        spriteRenderer.sprite = damageSpriteCoal;
        hp -= loss;

        if (hp <= 0) {
			p.coal += 2;
			p.movePoints -= 1;
            gameObject.SetActive (false);
        }
    }
}
