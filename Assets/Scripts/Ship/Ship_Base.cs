using UnityEngine;
using System;

public class Ship_Base : BaseObj {

  protected override void Init() {
    Sprite.StartBlur(0.1f, 0.4f, 0.015f);
  }

}
