using UnityEngine;
using System.Collections;

public class Enemy : MovingObject {
  public int playerDamage;
  private Animator animator;
  private Transform target;
  private bool skipMove;

  protected override void Start () {
      GameManager.instance.AddEnemyToList (this);
      animator = GetComponent<Animator> ();
      target = GameObject.FindGameObjectWithTag ("Player").transform;
      base.Start ();
  }

  protected override void AttemptMove <T> (int xDir, int yDir) {
      //Check if skipMove is true, if so set it to false and skip this turn.
      if(skipMove) {
          skipMove = false;
          return;
      }

      base.AttemptMove <T> (xDir, yDir);
      skipMove = true;
  }

  public void MoveEnemy () {
    //Declare variables for X and Y axis move directions, these range from -1 to 1.
    //These values allow us to choose between the cardinal directions: up, down, left and right.
    int xDir = 0;
    int yDir = 0;

    if(Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon)
        yDir = target.position.y > transform.position.y ? 1 : -1;
    else
        xDir = target.position.x > transform.position.x ? 1 : -1;

    //Call the AttemptMove function and pass in the generic parameter Player, because Enemy is moving and expecting to potentially encounter a Player
    AttemptMove <Player> (xDir, yDir);
  }


  protected override void OnCantMove <T> (T component) {
    Player hitplayer = component as Player;
    animator.SetTrigger ("enemyAttack");
    hitplayer.LoseFood (playerDamage);
    Player targetScript = target.gameObject.GetComponent <Player> ();
  }

  protected override void EnemyHitWall <T> (T component) {
  }

  public void TakeDamage (int damage) {
  }

  protected override void EnemyAttack <T> (T component) {
  }
}
