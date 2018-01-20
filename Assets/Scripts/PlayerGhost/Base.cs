using UnityEngine;
using System;
using System.Collections;

namespace PlayerGhost {
  public class Base : BaseObj {
    [Tooltip("The amount of speed the player picks up per step")]
    public Vector2 accelerationSpeed = new Vector2(0.1f, 0.05f);

    private bool spinning = true;

    public override void LoadReferences() {
      Sprite = new Sprite();
      Sound = new Sound();
      Input = new Input();
      Physics = new Physics(Physics);
      base.LoadReferences();
    }

    public override void Init() {
      StartCoroutine("StopSpinning");
      StartCoroutine("DelayParticles");
      Game.Create("Collector", Position);
      base.Init();
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
