using System.Collections;
using System.Collections.Generic;

public class Target_SFXLib : SFXLib {
  public override List<Clip> GetSounds() {
    return new List<Clip>() {
      new Clip("TargetBreak", 1),
    };
  }
}
