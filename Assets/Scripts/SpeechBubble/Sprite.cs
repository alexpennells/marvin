using UnityEngine;
using System;

namespace SpeechBubble {
  public class Sprite : SpriteObj {
    public void PlayOpen() {
      Animate("open", 1f);
    }

    public void PlayClose() {
      Animate("close", 1f);
    }
  }
}
