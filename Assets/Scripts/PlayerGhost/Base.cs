using UnityEngine;
using System;
using System.Collections;

namespace PlayerGhost {
  public class Base : InputObj {
    [Tooltip("The amount of speed the player picks up per step")]
    public Vector2 accelerationSpeed = new Vector2(0.1f, 0.05f);

    private bool spinning = true;

    protected override void LoadReferences() {
      Sprite = new Sprite();
      Sprite.enabled = true;
      base.LoadReferences();
    }

    protected override void Init() {
      StartCoroutine("StopSpinning");
      StartCoroutine("DelayParticles");
      Game.Create("Collector", Position);
    }

    protected override void LeftHeld (float val) {
      if (!Is("Spinning"))
        Physics.hspeed -= accelerationSpeed.x;
    }

    protected override void RightHeld (float val) {
      if (!Is("Spinning"))
        Physics.hspeed += accelerationSpeed.x;
    }

    protected override void UpHeld (float val) {
      if (!Is("Spinning"))
        Physics.vspeed += accelerationSpeed.y;
    }

    protected override void DownHeld (float val) {
      if (!Is("Spinning"))
        Physics.vspeed -= accelerationSpeed.y;
    }

    /***********************************
     * STATE CHECKERS
     **********************************/

    public bool IsSpinning() { return spinning; }

    /***********************************
     * CO-ROUTINES
     **********************************/

    private IEnumerator DelayParticles() {
      yield return new WaitForSeconds(0.15f);
      Game.CreateParticle("PlayerDie", Position);
    }

    private IEnumerator StopSpinning() {
      yield return new WaitForSeconds(1.75f);
      Physics.hspeedMax = 2f;
      spinning = false;
    }
  }
}
