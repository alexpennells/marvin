using UnityEngine;
using System;

public class Wisp_Base : BaseObj {
  private BaseObj follow = null;
  private Player_Base player;

  private ParticleSystem fire;
  new private Light light;

  public float maxDist = 0.1f;

  protected override void LoadReferences() {
    this.fire = transform.Find("Fire").gameObject.GetComponent<ParticleSystem>();
    this.light = this.fire.transform.Find("Light").gameObject.GetComponent<Light>();
  }

  protected override void Init() {
    this.player = GameObject.Find("Player").GetComponent<Player_Base>();
    this.follow = this.player;
    JumpToDesiredPosition();
  }

  protected override void Step () {
    MoveToDesiredPosition();

    EnemyObj closestEnemy = null;
    for (int i = 0; i < Game.Instance.ActiveEnemies.Count; ++i) {
      EnemyObj curEnemy = Game.Instance.ActiveEnemies[i];
      if (VectorLib.GetDistance(curEnemy, this.player) < 2f) {
        closestEnemy = curEnemy;
      }
    }

    if (closestEnemy == null) {
      ChangeColor("White");
      this.follow = this.player;
    } else {
      ChangeColor("Red");
      this.follow = closestEnemy;
    }
  }

  // Instantly jump to the desired position.
  public void JumpToDesiredPosition() {
    Position = DesiredPosition();
  }

  // Gradually move to the desired position.
  private void MoveToDesiredPosition() {
    Vector3 des = DesiredPosition();

    if (x < des.x)
      Physics.hspeed += 0.015f;
    else
      Physics.hspeed -= 0.015f;

    if (y < des.y)
      Physics.vspeed += 0.01f;
    else
      Physics.vspeed -= 0.01f;

    // Snap X if out of bounds
    if (x < MinimumDesiredX())
      x += (MinimumDesiredX() - x) / 24;
    else if (x > MaximumDesiredX())
      x -= (x - MaximumDesiredX()) / 24;

    // Snap Y if out of bounds
    if (y < MinimumDesiredY())
      y += (MinimumDesiredY() - y) / 16;
    else if (y > MaximumDesiredY())
      y -= (y - MaximumDesiredY()) / 16;
  }

  private Vector3 DesiredPosition () {
    if (this.follow == null)
      return Position;

    float yOffset = 0.25f;
    float xOffset = 0.25f;
    if (this.follow.Sprite.FacingLeft)
      xOffset *= -1;

    return new Vector3(this.follow.Position.x + xOffset, this.follow.Position.y + yOffset, this.follow.Position.z);
  }

  private float MinimumDesiredX() {
    return DesiredPosition().x - this.maxDist;
  }

  private float MaximumDesiredX() {
    return DesiredPosition().x + this.maxDist;
  }

  private float MinimumDesiredY() {
    return DesiredPosition().y - this.maxDist;
  }

  private float MaximumDesiredY() {
    return DesiredPosition().y + this.maxDist;
  }

  private void ChangeColor(string color) {
    var fireman = this.fire.main;
    switch (color) {
      case "White":
        fireman.startColor = new Color(255, 255, 255);
        this.light.color = new Color(1, 1, 1);
        break;
      case "Red":
        fireman.startColor = new Color(255, 0, 0);
        this.light.color = new Color(1, 0.5f, 0.5f);
        break;
      case "Blue":
        fireman.startColor = new Color(0, 0, 255);
        this.light.color = new Color(0, 0, 1);
        break;
      case "Green":
        fireman.startColor = new Color(0, 255, 0);
        this.light.color = new Color(0, 1, 0);
        break;
    }
  }

  /***********************************
   * STATE CHANGE FUNCTIONS
   **********************************/

  public void StateAggressive(BaseObj other) {
    this.follow = other;
  }

  /***********************************
   * EDITOR NONSENSE
   **********************************/

  protected override bool DrawGizmos() {
    // Desired Position
    Debug.DrawLine(new Vector3(DesiredPosition().x - Game.UNIT / 16, DesiredPosition().y, z), new Vector3(DesiredPosition().x + Game.UNIT / 16, DesiredPosition().y, z), Color.magenta, 0, false);
    Debug.DrawLine(new Vector3(DesiredPosition().x, DesiredPosition().y - Game.UNIT / 16, z), new Vector3(DesiredPosition().x, DesiredPosition().y + Game.UNIT / 16, z), Color.magenta, 0, false);

    Debug.DrawLine(new Vector3(MinimumDesiredX(), MaximumDesiredY(), z), new Vector3(MaximumDesiredX(), MaximumDesiredY(), z), Color.magenta, 0, false);
    Debug.DrawLine(new Vector3(MinimumDesiredX(), MinimumDesiredY(), z), new Vector3(MaximumDesiredX(), MinimumDesiredY(), z), Color.magenta, 0, false);
    Debug.DrawLine(new Vector3(MinimumDesiredX(), MinimumDesiredY(), z), new Vector3(MinimumDesiredX(), MaximumDesiredY(), z), Color.magenta, 0, false);
    Debug.DrawLine(new Vector3(MaximumDesiredX(), MinimumDesiredY(), z), new Vector3(MaximumDesiredX(), MaximumDesiredY(), z), Color.magenta, 0, false);

    return false;
  }
}
