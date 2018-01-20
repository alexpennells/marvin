using UnityEngine;
using System;
using System.Collections;

namespace PlayerHead {
  public class Base : BaseObj {
    private Player.Base player;

    public override void LoadReferences() {
      Sprite = new Sprite();
      base.LoadReferences();
    }

    public override void Init () {
      player = GameObject.Find("Player").GetComponent<Player.Base>() as Player.Base;
      base.Init();
    }

    /***********************************
     * STATE CHANGE FUNCTIONS
     **********************************/

    public void StateRevive() {
      Physics.gravity = 0;
      Sprite.Play("Revive");
    }

    public void StateReturn() {
      Sprite.Play("Alive");
      player.Sprite.Play("Revive");
      StartCoroutine("Return");
    }

    /***********************************
     * CO-ROUTINES
     **********************************/

    private IEnumerator Return() {
      Vector3 start = Position;
      float stepSize = 1 / (Math.Abs(start.x - player.x) * 10 + 25);
      for (float t = 0; t <= 1f; t = t + stepSize) {
        Position = Vector3.Lerp(start, player.Mask.Center + new Vector3(0, 0.05f, 0), t);

        if (t <= 0.5f)
          transform.localScale = new Vector3(1 + t * 2, 1 + t, 1);
        else
          transform.localScale = new Vector3(3 - t * 2, 2 - t, 1);

        yield return new WaitForSeconds(0.01f);
      }

      player.State("Revive");
      DestroySelf();
      Game.Camera.LoadFocus();
    }
  }
}
