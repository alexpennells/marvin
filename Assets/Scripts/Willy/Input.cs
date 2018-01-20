using UnityEngine;
using System;

namespace Willy {
  public class Input : InputBlock {
    public Input() { enabled = true; }

    protected override void DosPressed () {
      Base.State("Punch");
    }
  }
}
