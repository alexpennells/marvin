using System.Collections;
using System.Collections.Generic;

namespace Rat {
  public class SFX : SFXLib {
    public override List<Clip> GetSounds() {
      return new List<Clip>() {
        new Clip("RatSqueak", 1),
        new Clip("RatDie", 1),
      };
    }
  }
}
