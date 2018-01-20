using UnityEngine;
using System;

public class Circle_Ladder : Ladder_Base
{
  // The radius of the platform from the center axis.
  public float radius = 0f;

  // The rotation speed around the center axis.
  public float speed = 0f;

  // The angle about the center axis.
  public float angle = 0f;

  private float centerX, centerY;

  public override void Init() {
    centerX = x;
    centerY = y;
  }

  protected override void Move() {
    angle += speed;
    MoveAbsolute(centerX + radius * (float)Math.Cos(angle), centerY + radius * (float)Math.Sin(angle));
  }
}
