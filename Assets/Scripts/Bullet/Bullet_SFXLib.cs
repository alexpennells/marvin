using System.Collections;
using System.Collections.Generic;

public class Bullet_SFXLib : SFXLib {
  public override List<Clip> GetSounds() {
    return new List<Clip>() {
      new Clip("BulletPop", 1),
      new Clip("BulletShoot", 1),
    };
  }
}
