using System.Collections;
using System.Collections.Generic;

namespace Coin {
  public class SFX : SFXLib {
    public override List<Clip> GetSounds() {
      return new List<Clip>() {
        new Clip("CollectCoin", 2),
      };
    }
  }
}
