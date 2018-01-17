using System;

namespace Coin {
  public class Sound : SoundObj {
    public override Type Lib { get { return typeof(Coin.SFX); } }

    public void PlayCollect() {
      Game.SFX.Play("CollectCoin" + Variation(2), 0.5f);
    }
  }
}
