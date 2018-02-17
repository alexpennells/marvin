using System;
using UnityEngine;

namespace AmmoBall {
  public class Sound : SoundBlock {
    public Sound() { enabled = true; }
    public override Type Lib { get { return typeof(AmmoBall.SFX); } }

    public void PlayBounce(float volume) {
      Game.SFX.Play("BallBounce" + Variation(4), volume);
    }
  }
}
