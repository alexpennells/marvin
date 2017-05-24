using UnityEngine;
using System;
using System.Timers;
using System.Collections.Generic;

public class Worm_Base : BaseObj {
  [Tooltip("Number of body segments on this worm")]
  public int length = 3;

  [Tooltip("Number of steps between body pieces")]
  public int bodyDistance = 10;

  public float RotationAngle { get { return rotationAngle; } }
  private float rotationAngle = 0f;

  private Queue<WormBody_Base> bodies = new Queue<WormBody_Base>();
  private Queue<WormState> states = new Queue<WormState>();
  private int health = 3;

  protected override void Init() {
    HurtTimer.Interval = 250;

    // Initialize the body pieces.
    for (int i = 0; i < this.length; ++i) {
      WormBody_Base part = Game.Create("WormBody", Position) as WormBody_Base;
      part.UpdateState(GetState());
      part.Sprite.SetLayer(-length - 1 + i);
      this.bodies.Enqueue(part);
    }

    for (int i = 0; i < StateLength(); ++i) {
      this.states.Enqueue(GetState());
    }
  }

  protected override void Step () {
    this.rotationAngle = Physics.vspeed * 25;
    if (Sprite.FacingLeft)
      this.rotationAngle *= -1;

    this.UpdateStates();
    this.UpdateBody();

    if (Is("Hurt"))
      Physics.SkipNextUpdate();
  }

  private void UpdateStates () {
    this.states.Dequeue();
    this.states.Enqueue(GetState());
  }

  private void UpdateBody () {
    // Update the body pieces.
    WormState[] stateArray = states.ToArray();
    int bodyIndex = 0;

    foreach (WormBody_Base b in bodies) {
      b.UpdateState(stateArray[bodyIndex * bodyDistance]);
      bodyIndex++;
    }
  }

  private WormState GetState() {
    return new WormState(Position, Sprite.FacingLeft, rotationAngle);
  }

  private int StateLength () {
    return this.length * this.bodyDistance;
  }

  private void DequeueBody () {
    this.bodies.Dequeue().DestroySelf();

    for (int i = 0; i < bodyDistance; ++i)
      this.states.Dequeue();
    length -= 1;
  }

  /***********************************
   * STATE CHANGE FUNCTIONS
   **********************************/

  public void StateHurt(int damage) {
    HurtTimer.Enabled = false;
    Sprite.Play("Red");
    health -= damage;

    if (health <= 0) {
      if (length <= 1) {
        Game.CreateParticle("WormDie", Mask.Center);
        DequeueBody();
        DestroySelf();
      } else {
        Game.CreateParticle("WormBodyDie", Mask.Center);
        health = 3;
        DequeueBody();
      }
    }

    HurtTimer.Enabled = true;
  }

  /***********************************
   * STATE CHECKERS
   **********************************/

  public bool IsHurt() { return HurtTimer.Enabled; }

  /***********************************
   * TIMER HANDLERS
   **********************************/

  public Timer HurtTimer { get { return Timer5; } }
  protected override void Timer5Elapsed(object source, ElapsedEventArgs e) {
    HurtTimer.Enabled = false;
  }
}
