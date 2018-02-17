using UnityEngine;
using System;

namespace Minion {
  public class Base : BaseObj {
    public float acceleration = 0.2f;

    public float relativeMinPosition = 1f;
    public float relativeMaxPosition = 1f;
    public float MinPosition { get { return this.StartPosition.x + relativeMinPosition; } }
    public float MaxPosition { get { return this.StartPosition.x + relativeMaxPosition; } }

    private eDirection direction;
    private bool dead = false;

    public override void LoadReferences() {
      Sprite = new Sprite(Sprite);
      Sound = new Sound();
      Physics = new Physics(Physics);
      base.LoadReferences();
    }

    public override void Init() {
      if (Physics.hspeed > 0)
        direction = eDirection.RIGHT;
      else if (Physics.hspeed < 0)
        direction = eDirection.LEFT;
      else if (Sprite.mirrorToStart)
        direction = eDirection.LEFT;
      else
        direction = eDirection.RIGHT;

      base.Init();
    }

    public override void Step() {
      if (!Is("Dead")) {
        if (direction == eDirection.LEFT && x < MinPosition)
          direction = eDirection.RIGHT;
        else if (direction == eDirection.RIGHT && x > MaxPosition)
          direction = eDirection.LEFT;

        if (direction == eDirection.LEFT) {
          Physics.hspeed -= acceleration;
        } else {
          Physics.hspeed += acceleration;
        }
      }

      base.Step();
    }

    public void ChangeDirection() {
      if (direction == eDirection.LEFT)
        direction = eDirection.RIGHT;
      else
        direction = eDirection.LEFT;
    }

    public void CreateWalkPuffs(int count) {
      for (int i = 0; i < count; i++)
        Game.CreateParticle("Puff", Position);
    }

    public void FinishFallAnimation() {
      CreateWalkPuffs(10);
    }

    /***********************************
     * STATE CHANGE FUNCTIONS
     **********************************/

    public void StateDie() {
      dead = true;
      Physics.friction = 0.05f;
      Sprite.Play("Fall");
      Sound.Play("Laugh");

      BaseObj skull = Game.Create("Skull", Mask.Center);
      skull.Physics.hspeed = Sprite.FacingLeft ? -1 : 1;
      skull.Physics.vspeed = 1;
      skull.State("Invincible");
    }

    /***********************************
     * STATE CHECKERS
     **********************************/

    public bool IsDead() { return dead; }

    /***********************************
     * CO-ROUTINES
     **********************************/

    protected override bool DrawGizmos() {
      Debug.DrawLine(new Vector3(MinPosition, Mask.Center.y - Game.UNIT/2, z), new Vector3(MinPosition, Mask.Center.y + Game.UNIT/2, z), Color.magenta, 0, false);
      Debug.DrawLine(new Vector3(MaxPosition, Mask.Center.y - Game.UNIT/2, z), new Vector3(MaxPosition, Mask.Center.y + Game.UNIT/2, z), Color.magenta, 0, false);

      return true;
    }
  }
}
