using System.Collections;
using System.Collections.Generic;

namespace Target {
  public class SFX : SFXLib {
    public override List<Clip> GetSounds() {
      return new List<Clip>() {
        new Clip("TargetBreak", 1),
        new Clip("TargetMonster", 1),
      };
    }
  }
}
