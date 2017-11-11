using UnityEngine;
using System;

namespace SpeechBubble {
  [RequireComponent (typeof (Sprite))]
  public class Base : BaseObj {
    [Tooltip("The text played when interacting with this speech bubble")]
    public string[] text = new string[1];

    private Teleprompter teleprompter;

    protected override void LoadReferences() {
      this.teleprompter = GameObject.Find("Teleprompter").GetComponent<Teleprompter>();
    }

    public void StatePlayText() {
      if (Game.paused)
        return;

      foreach (string t in text)
        this.teleprompter.text.Enqueue(t);
      this.teleprompter.PrintText();
    }
  }
}
