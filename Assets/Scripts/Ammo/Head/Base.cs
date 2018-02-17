using UnityEngine;
using System;
using System.Collections;

namespace AmmoHead {
  public class Base : BaseObj {
    private Player.Base player;
    private bool collectable = false;

    public override void LoadReferences() {
      player = GameObject.Find("Player").GetComponent<Player.Base>() as Player.Base;

      Physics = new Physics(Physics);
      Sprite = new Sprite();
      base.LoadReferences();
    }

    public override void Init() {
      StartCoroutine("Collectable");
      StartCoroutine("ReturnToTig");
      base.Init();
    }

    public override void Step () {
      if (!HasFooting) { Physics.SkipNextFrictionUpdate(); }
      base.Step();
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

    private IEnumerator ReturnToTig() {
      yield return new WaitForSeconds(1.5f);
      Physics.enabled = false;

      for (float t = 0; t < 0.8f; t += 0.01f) {
        Position = Vector3.Lerp(Position, player.Mask.Center, t);
        yield return null;
      }

      Stitch.AddToAmmo(eAmmoType.HEAD);
      DestroySelf();
    }
  }
}
