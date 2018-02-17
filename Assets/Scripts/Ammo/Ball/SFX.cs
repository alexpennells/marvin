using System.Collections;
using System.Collections.Generic;

namespace AmmoBall {
  public class SFX : SFXLib {
    public override List<Clip> GetSounds() {
      return new List<Clip>() {
        new Clip("BallBounce", 4)
      };
    }
  }
}
