using UnityEngine;
using System;
using System.Collections;

namespace Collector {
  public class Base : BaseObj {
    private Player.Base player;
    private PlayerHead.Base head;
    private bool flyAway = false;

    public override void LoadReferences() {
      Sprite = new Sprite();
      base.LoadReferences();
    }

    public override void Init() {
      player = GameObject.Find("Player").GetComponent<Player.Base>() as Player.Base;

      transform.localScale = Vector3.zero;
      x = player.x - 1.2f;
      y = player.y - 0.3f;

      StartCoroutine("Grow");
      Game.CreateParticle("CollectorSmoke", Position);
      base.Init();
    }

    public override void Step() {
      if (flyAway) {
        if (x > Game.Scene.ClearPathMax())
          Sprite.FacingLeft = true;
        else if (x < Game.Scene.ClearPathMin())
          Sprite.FacingRight = true;

        if (Sprite.FacingLeft)
          Physics.hspeed -= 0.05f;
        else
          Physics.hspeed += 0.05f;

        if (y < Game.Scene.ClearPathAt(x))
          Physics.vspeed += 0.025f;
        else
          Physics.vspeed -= 0.025f;
      }

      base.Step();
    }

    public void Decapitate() {
      player.Sprite.Play("Die");
      head = Game.Create("PlayerHead", player.Mask.Center) as PlayerHead.Base;
      head.Physics.vspeed = 2;
      StartCoroutine("CollectHead");
    }

    /***********************************
     * STATE CHANGE FUNCTIONS
     **********************************/

    public void StateDisappear() {
      Physics.hspeed = 0;
      Physics.vspeed = 0;
      flyAway = false;
      Sprite.Play("HeadlessFly");
      StartCoroutine("Shrink");
    }

    /***********************************
     * CO-ROUTINES
     **********************************/

    private IEnumerator Grow() {
      Vector3 start = Position;
      for (float t = 0; t <= 1f; t = t + 0.025f) {
        transform.localScale = new Vector3(t, t, 1);
        Position = Vector3.Lerp(start, player.Position + new Vector3(-0.3f, 0.3f, 0), t);
        yield return new WaitForSeconds(0.01f);

        if (t > 0.9f)
          Sprite.SetSpeed(0.6f);
      }
    }

    private IEnumerator Shrink() {
      yield return new WaitForSeconds(1f);

      Vector3 start = Position;
      for (float t = 0; t <= 1f; t = t + 0.05f) {
        transform.localScale = new Vector3(1 - t, 1 - t, 1);
        Position = Vector3.Lerp(start, start + new Vector3(0.5f, -0.3f, 0), t);
        yield return new WaitForSeconds(0.01f);

        if (t > 0.75f) {
          Game.CreateParticle("CollectorSmoke", Position);
          DestroySelf();
        }
      }
    }

    private IEnumerator CollectHead() {
      Vector3 start = Position;
      for (float t = 0; t <= 1f; t = t + 0.025f) {
        Position = Vector3.Lerp(start, head.Position + new Vector3(0.2f, 0.25f, 0), t);
        yield return new WaitForSeconds(0.01f);
      }

      head.DestroySelf();
      Sprite.Play("Fly");
      flyAway = true;
    }
  }
}
