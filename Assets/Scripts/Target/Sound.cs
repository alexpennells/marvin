using UnityEngine;
using System;

namespace Target {
  public class Sound : SoundObj {
    private float lastPlayedAppear = 0;

    public override Type Lib { get { return typeof(Target.SFX); } }

    public void PlayBreak() {
      Game.SFX.Play("TargetBreak", 1f);
    }

    public void PlayAppear() {
      float curTime = Time.fixedTime;
      if (Math.Abs(curTime - lastPlayedAppear) > 2f) {
        Game.SFX.Play("TargetMonster", 0.1f);
        lastPlayedAppear = curTime;
      }
    }

    /***********************************
     * LOOPS
     **********************************/

    public void StartWind() {
      Loops["Wind"].volume = 0.25f;
    }

    public void StopWind() {
      Loops["Wind"].volume = 0f;
    }
  }
}
