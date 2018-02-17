using UnityEngine;
using System;
using System.Collections;

namespace PlayerGhostBody {
  public class Base : BaseObj {
    public float delay = 0.5f;
    public Bullet.Base Head { get {return head;} set {head = value;}}
    private Bullet.Base head;

    public override void Init() {
      StartCoroutine("Absorb");
      Physics.hspeed = Game.Random.Next(100, 300) / 100f;
      Physics.vspeed = Game.Random.Next(100, 300) / 100f;

      if (Game.Random.Next(1, 3) == 1)
        Physics.hspeed = -Physics.hspeed;

      if (Game.Random.Next(1, 3) == 1)
        Physics.vspeed = -Physics.vspeed;
    }

    public override void Step() {
      base.Step();
    }

    /***********************************
     * STATE CHECKERS
     **********************************/

    /***********************************
     * CO-ROUTINES
     **********************************/

    private IEnumerator Absorb() {
      yield return new WaitForSeconds(delay);
      Physics.enabled = false;

      for (float t = 0; t < 0.4f; t += 0.05f) {
        Position = Vector3.Lerp(Position, head.Position, t);
        yield return null;
      }

      head.RestartCoroutine("AbsorbGhost");
      DestroySelf();
    }
  }
}
