using System.Collections;
using System.Collections.Generic;

namespace Soul {
  public class SFX : SFXLib {
    public override List<Clip> GetSounds() {
      return new List<Clip>() {
        new Clip("CollectSoul", 3),
      };
    }
  }
}
