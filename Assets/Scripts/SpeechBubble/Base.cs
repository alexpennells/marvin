using UnityEngine;
using System;

namespace SpeechBubble {
  public class Base : BaseObj {
    [Tooltip("The text played when interacting with this speech bubble")]
    public string[] text = new string[1];

    private Teleprompter teleprompter;

    protected override void LoadReferences() {
      this.teleprompter = GameObject.Find("Teleprompter").GetComponent<Teleprompter>();

      Sprite = new Sprite();
      Sprite.enabled = true;
      base.LoadReferences();
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
