using System.Collections;
using System.Collections.Generic;

public class Player_SFXLib : SFXLib {
  public override List<Clip> GetSounds() {
    return new List<Clip>() {
      new Clip("Footstep", 6),
      new Clip("CatJump", 5),
    };
  }
}
