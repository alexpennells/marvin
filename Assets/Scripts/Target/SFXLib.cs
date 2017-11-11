using System.Collections;
using System.Collections.Generic;

namespace Target {
  public class Target_SFXLib : SFXLib {
    public override List<Clip> GetSounds() {
      return new List<Clip>() {
        new Clip("TargetBreak", 1),
      };
    }
  }
}
