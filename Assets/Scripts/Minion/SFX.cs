using System.Collections;
using System.Collections.Generic;

namespace Minion {
  public class SFX : SFXLib {
    public override List<Clip> GetSounds() {
      return new List<Clip>() {
        new Clip("MinionStep", 3),
      };
    }
  }
}
