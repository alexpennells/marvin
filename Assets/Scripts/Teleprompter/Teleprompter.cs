using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Teleprompter : MonoBehaviour {
  [Tooltip("The material to be applied to the printed characters")]

  public Material charMaterial;
  public Queue<string> text = new Queue<string>();

  // The top left position of the text grid.
  private Vector3 origin = new Vector3(1.05f, 6.04f, 0);

  // Whether we're waiting for a keypress to continue/end the conversation.
  private bool waitingOnSkull = false;

  // The current position to draw the next character to.
  private Vector3 curPos;

  // Whether we're currently displaying the teleprompter.
  private bool isPrinting = false;

  // Default formatting options.
  private float defaultTextSpeed = 0.01f;
  private Color defaultTextColor = Color.white;
  private float defaultFontSize = 0.6f;
  private float defaultLetterSpacing = 0.04f;
  private float defaultLineSpacing = 0.08f;

  // Formatting options.
  private float textSpeed;
  private Color textColor;
  private float fontSize;
  private float letterSpacing;
  private float lineSpacing;

  // Component/asset References
  private Sprite[] font;
  private GameObject skull;
  private SpriteRenderer backdrop;

  void Awake() {
    font = Resources.LoadAll<Sprite>("Font");
  }

  void Start() {
    skull = this.transform.Find("Skull").gameObject;
    backdrop = this.transform.Find("Back").gameObject.GetComponent<SpriteRenderer>();
  }

  void Update() {
    if (Input.GetButtonDown(Game.UNO)) {
      if (waitingOnSkull)
        SkullPressed();
      else
        this.textSpeed = 0;
    }
  }

  // Print the current queue of text.
  // 'cont' stands for continue and will override the isPrinting check
  public void PrintText(bool cont = false) {
    if (cont || !isPrinting) {
      isPrinting = true;
      textColor = Color.white;
      Game.paused = true;
      Game.pausedByPlayer = false;
      ClearText();
      StartCoroutine("crPrintText");
      skull.SetActive(false);
      waitingOnSkull = false;
    }
  }

  private void SkullPressed() {
    if (text.Count != 0)
      PrintText(true);
    else {
      isPrinting = false;
      skull.SetActive(false);
      ClearText();
      Game.paused = false;
      StartCoroutine("crFadeOutBackdrop");
    }
  }

  private void ClearText() {
    foreach (Transform child in this.transform.Find("Text"))
      GameObject.Destroy(child.gameObject);
  }

  private void PrintChar(char c, Vector3 pos) {
    GameObject newChar = new GameObject();
    newChar.transform.parent = this.transform.Find("Text");
    newChar.transform.localPosition = pos;
    newChar.transform.localScale = new Vector3(fontSize, fontSize, 0);

    SpriteRenderer rend = newChar.AddComponent<SpriteRenderer>() as SpriteRenderer;
    rend.sprite = font[FontIndex.Get(c)];
    rend.color = this.textColor;
    rend.sortingLayerName = "HUD";
    rend.sortingOrder = 2;

    if (charMaterial != null)
      rend.material = charMaterial;

    if (c == ' ')
      rend.color = new Color(1, 1, 1, 0);
  }

  // Apply the different text commands to the printed text.
  private float HandleCommand(string cmd) {
    switch (cmd) {
      case "br":
        this.curPos = new Vector3(origin.x, curPos.y - lineSpacing, 0);
        return this.textSpeed;
      case "white":
        this.textColor = Color.white;
        break;
      case "red":
        this.textColor = Color.red;
        break;
      case "green":
        this.textColor = Color.green;
        break;
      case "cyan":
        this.textColor = Color.cyan;
        break;
      case "magenta":
        this.textColor = Color.magenta;
        break;
      case "yellow":
        this.textColor = Color.yellow;
        break;
      case "lg":
        this.fontSize = 0.8f;
        this.letterSpacing = 0.06f;
        break;
      case "md":
        this.fontSize = this.defaultFontSize;
        this.letterSpacing = this.defaultLetterSpacing;
        break;
      case "sm":
        this.fontSize = 0.4f;
        this.letterSpacing = 0.03f;
        break;
    }

    return 0;
  }

  private void resetToDefaultFormatting() {
    this.textSpeed = this.defaultTextSpeed;
    this.textColor = this.defaultTextColor;
    this.fontSize = this.defaultFontSize;
    this.letterSpacing = this.defaultLetterSpacing;
    this.lineSpacing = this.defaultLineSpacing;
  }

  /*************************************************************
   * CO-ROUTINES
   ************************************************************/

  private IEnumerator crFadeOutBackdrop() {
    while (backdrop.color.a > 0) {
      backdrop.color = new Color(1, 1, 1, backdrop.color.a - 0.05f);
      yield return null;
    }
  }

  private IEnumerator crPrintText() {
    // Fade in backdrop.
    while (backdrop.color.a < 0.7f) {
      backdrop.color = new Color(1, 1, 1, backdrop.color.a + 0.05f);
      yield return null;
    }

    this.curPos = origin;
    this.resetToDefaultFormatting();
    string printing = text.Dequeue();

    for (int i = 0; i < printing.Length; ++i) {
      if (printing[i] != '|') {
        PrintChar(printing[i], curPos);
        curPos += new Vector3(letterSpacing, 0f, 0f);

        if (curPos.x > origin.x + 2.15f)
          curPos = new Vector3(origin.x, curPos.y - lineSpacing, 0);

        if (this.textSpeed != 0)
          yield return new WaitForSeconds(this.textSpeed);
      } else {
        // Handle formatting commands between |these| vertical brackets.
        string cmd = "";
        i++;
        while (printing[i] != '|')
          cmd = cmd + printing[i++];

        float waitTime = HandleCommand(cmd);
        if (waitTime != 0)
          yield return new WaitForSeconds(waitTime);
      }
    }

    skull.SetActive(true);
    waitingOnSkull = true;
  }

}
