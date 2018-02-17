using UnityEngine;
using System;
using System.Collections;

namespace PlayerGhost {
  public class Base : BaseObj {
    [Tooltip("The amount of speed the player picks up per step")]
    public Vector2 accelerationSpeed = new Vector2(0.1f, 0.05f);
    public bool controllable = false;

    private bool spinning = true;

    public Bullet.Base Head { get {return head;} set {head = value;}}
    private Bullet.Base head;

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
      Input.enabled = false;
      base.Init();
    }

    public override void Step() {
      if (!controllable && !spinning) {
        if (x > Game.Scene.ClearPathMax())
          Sprite.FacingLeft = true;
        else if (x < Game.Scene.ClearPathMin())
          Sprite.FacingRight = true;

        if (Sprite.FacingLeft)
          Physics.hspeed -= accelerationSpeed.x;
        else
          Physics.hspeed += accelerationSpeed.x;

        if (y < Game.Scene.ClearPathAt(x))
          Physics.vspeed += accelerationSpeed.y;
        else
          Physics.vspeed -= accelerationSpeed.y;
      }

      base.Step();
    }

    public override void DestroySelf() {
      (Game.Create("PlayerGhostBody/Head", Mask.Center) as PlayerGhostBody.Base).Head = head;
      (Game.Create("PlayerGhostBody/Body", Mask.Center) as PlayerGhostBody.Base).Head = head;
      (Game.Create("PlayerGhostBody/FrontArm", Mask.Center) as PlayerGhostBody.Base).Head = head;
      (Game.Create("PlayerGhostBody/BackArm", Mask.Center) as PlayerGhostBody.Base).Head = head;
      (Game.Create("PlayerGhostBody/Tail", Mask.Center) as PlayerGhostBody.Base).Head = head;
      base.DestroySelf();
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
      yield return new WaitForSeconds(1.5f);
      Physics.hspeedMax = 1f;
      Physics.vspeedMax = 1f;
      spinning = false;
      Sprite.FacingLeft = !Sprite.FacingLeft;

      if (controllable)
        Input.enabled = true;
    }
  }
}
