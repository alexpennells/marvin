using UnityEngine;
using System;
using System.Timers;

namespace Minion {
  [RequireComponent (typeof (Sprite))]
  [RequireComponent (typeof (Sound))]
  public class Base : EnemyObj {
    public float acceleration = 0.2f;

    public float relativeMinPosition = 1f;
    public float relativeMaxPosition = 1f;
    public float MinPosition { get { return this.StartPosition.x + relativeMinPosition; } }
    public float MaxPosition { get { return this.StartPosition.x + relativeMaxPosition; } }

    private eDirection direction;

    protected override void Init() {
      if (Physics.hspeed > 0)
        direction = eDirection.RIGHT;
      else if (Physics.hspeed < 0)
        direction = eDirection.LEFT;
      else if (Sprite.mirrorToStart)
        direction = eDirection.LEFT;
      else
        direction = eDirection.RIGHT;
    }

    protected override void Step() {
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

    public void ChangeDirection() {
      if (direction == eDirection.LEFT)
        direction = eDirection.RIGHT;
      else
        direction = eDirection.LEFT;
    }

    protected override bool DrawGizmos() {
      Debug.DrawLine(new Vector3(MinPosition, Mask.Center.y - Game.UNIT/2, z), new Vector3(MinPosition, Mask.Center.y + Game.UNIT/2, z), Color.magenta, 0, false);
      Debug.DrawLine(new Vector3(MaxPosition, Mask.Center.y - Game.UNIT/2, z), new Vector3(MaxPosition, Mask.Center.y + Game.UNIT/2, z), Color.magenta, 0, false);

      return true;
    }
  }
}
