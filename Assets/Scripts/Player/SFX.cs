using System.Collections;
using System.Collections.Generic;

namespace Player {
  public class SFX : SFXLib {
    public override List<Clip> GetSounds() {
      return new List<Clip>() {
        new Clip("Footstep", 6),
        new Clip("CatJump", 5),
        new Clip("Thump", 1),
        new Clip("Pound", 1),
      };
    }
  }
}
