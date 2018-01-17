using UnityEngine;
using System;
using System.Collections;

namespace Soul {
  public class Base : BaseObj {
    private bool collectable = false;

    protected override void LoadReferences() {
      Sprite = new Sprite();
      Sprite.enabled = true;
      base.LoadReferences();
    }

    protected override void Init () {
      Physics.hspeed = Game.Random.Next(0, 400) / 100f - 2f;
      Physics.vspeed = Game.Random.Next(0, 100) / 100f - 0.5f;
      StartCoroutine("Collectable");
    }

    protected override void Step () {
      Physics.vspeed += 0.005f;
      if (Math.Abs(Physics.hspeed) < 0.1f)
        Physics.SkipNextFrictionUpdate();
    }

    public override void DestroySelf() {
      Game.CreateParticle("CollectSoul", Position);
      Sound.Play("Collect");
      base.DestroySelf();
    }

    /***********************************
     * STATE CHECKERS
     **********************************/

    public bool IsCollectable() { return collectable; }

    /***********************************
     * CO-ROUTINES
     **********************************/

    private IEnumerator Collectable() {
      yield return new WaitForSeconds(0.5f);
      collectable = true;
    }
  }
}
