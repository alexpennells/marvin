using System.Collections;
using System.Collections.Generic;

namespace Balloon {
  public class SFX : SFXLib {
    public override List<Clip> GetSounds() {
      return new List<Clip>() {
        new Clip("BalloonPop", 1),
        new Clip("SoulsEscape", 1),
      };
    }
  }
}
