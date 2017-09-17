using UnityEngine;
using System;
using System.Timers;

public class SpeechBubble_Sprite : SpriteObj {
  public void PlayOpen() {
    Animate("open", 1f);
  }

  public void PlayClose() {
    Animate("close", 1f);
  }
}
