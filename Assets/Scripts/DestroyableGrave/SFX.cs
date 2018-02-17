using System.Collections;
using System.Collections.Generic;

namespace DestroyableGrave {
  public class SFX : SFXLib {
    public override List<Clip> GetSounds() {
      return new List<Clip>() {
        new Clip("GraveCrack", 1),
      };
    }
  }
}
