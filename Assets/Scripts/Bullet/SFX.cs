using System.Collections;
using System.Collections.Generic;

namespace Bullet {
  public class SFX : SFXLib {
    public override List<Clip> GetSounds() {
      return new List<Clip>() {
        new Clip("BulletPop", 1),
        new Clip("BulletShoot", 1),
      };
    }
  }
}
