using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Door {
  public class Base : Exit {
    public override void LoadReferences() {
      Sprite = new Sprite();
      base.LoadReferences();
    }

    public override void StateTransitionStart() {
      Game.ChangeScene(sceneName, exitID, transition);
      Sprite.Play("Open");
    }

    public override void StateTransitionMiddle(BaseObj player) {
      Sprite.Play("Close");

      player.Position = Position;
      player.Sprite.Play("Walk");
    }

    public override void StateTransitionEnd(BaseObj player) {
      Game.disableInput = false;
      Sprite.SetSpeed(0.5f);
    }
  }
}
