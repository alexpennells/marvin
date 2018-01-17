using System.Collections;
using System.Collections.Generic;

namespace Minion {
  public class SFX : SFXLib {
    public override List<Clip> GetSounds() {
      return new List<Clip>() {
        new Clip("MinionStep", 3),
        new Clip("MinionGiggle", 2),
        new Clip("MinionLaugh", 1),
        new Clip("MinionDie", 1),
      };
    }
  }
}
