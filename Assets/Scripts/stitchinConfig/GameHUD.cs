using UnityEngine;
using System;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

public class GameHUD : MonoBehaviour {
  private GameObject electricity;

  private Transform shieldBackdrop;
  private Transform shieldFront;
  private Transform shieldBar;
  private Transform damageBar;

  private GameObject pulse = null;

  private Timer regenTimer = new Timer();
  private Timer slideTimer = new Timer();

  /***********************************
   * UNITY FUNCTIONS
   **********************************/

  void Awake() {
    this.electricity = transform.Find("ShieldBackdrop").Find("Shield").Find("Electricity").gameObject;

    this.shieldBackdrop = transform.Find("ShieldBackdrop");
    this.shieldBar = transform.Find("ShieldBackdrop").Find("Shield").Find("Bar");
    this.damageBar = transform.Find("ShieldBackdrop").Find("Shield").Find("DamageBar");
    this.shieldFront = transform.Find("ShieldBackdrop").Find("Shield").Find("Front");

    SetShieldBar();
    SetDamageBar(0);
    SetColor(Stitch.CurShieldColor);
  }

  void Start() {
    regenTimer.Elapsed += new ElapsedEventHandler(RegenTimerElapsed);
    regenTimer.Interval = 2000;
    regenTimer.Enabled = false;

    slideTimer.Elapsed += new ElapsedEventHandler(SlideTimerElapsed);
    slideTimer.Interval = 2000;
    slideTimer.Enabled = false;
  }

  void Update() {
    RegenShield();
    UpdateDamageBar();
    SlideShieldBar();
    CreateLowShieldPulse();
  }

  /***********************************
   * PUBLIC FUNCTIONS
   **********************************/

  public void ReduceShield(int damage) {
    Stitch.Shield = (Stitch.CurShield == 0) ? 0 : Stitch.Shield - damage;

    if (Stitch.Shield <= 0) {
      GameObject p = Game.CreateParticle("ShieldDie", shieldFront.position);
      p.transform.parent = shieldFront;

      if (Stitch.CurShield > 0) {
        Stitch.CurShield -= 1;
        Stitch.Shield = Stitch.CurShield > 0 ? 100 : 1;
      }
    }

    ResetRegenTimer();
    SetDamageBar(this.shieldBar.localScale.x);
    SetShieldBar();
    SetColor(Stitch.CurShieldColor);
    ToggleElectricity();
  }

  /***********************************
   * PRIVATE HELPERS
   **********************************/

  private void CreateLowShieldPulse() {
    if (Stitch.CurShield != 0)
      return;

    if (this.pulse == null) {
      this.pulse = new GameObject(gameObject.name + "Pulse");
      this.pulse.transform.parent = this.shieldFront;
      this.pulse.transform.localPosition = Vector3.zero;

      SpriteRenderer spr = this.pulse.AddComponent<SpriteRenderer>() as SpriteRenderer;
      spr.sprite = this.shieldFront.GetComponent<SpriteRenderer>().sprite;
      spr.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.8f);
      spr.sortingLayerID = this.shieldFront.GetComponent<SpriteRenderer>().sortingLayerID;
      spr.sortingOrder = 11;
    } else {
      SpriteRenderer spr = this.pulse.GetComponent<SpriteRenderer>();
      spr.color = new Color(Color.red.r, Color.red.g, Color.red.b, spr.color.a - 0.0075f);
      this.pulse.transform.localScale = new Vector3(this.pulse.transform.localScale.x + 0.015f, this.pulse.transform.localScale.y + 0.015f, 1f);

      if (spr.color.a <= 0) {
        Destroy(this.pulse);
        this.pulse = null;
      }
    }
  }

  private void SlideShieldBar() {
    float desired = 0.5f;

    if (Stitch.Shield != 100) {
      slideTimer.Enabled = false;
      slideTimer.Enabled = true;
    }

    if (slideTimer.Enabled)
      desired = 0;

    if (this.shieldBackdrop.localPosition.y < desired) {
      this.shieldBackdrop.localPosition = new Vector3(0.5f, Math.Min(this.shieldBackdrop.localPosition.y + 0.01f, 0.5f), 0);
    } else if (this.shieldBackdrop.localPosition.y > desired) {
      this.shieldBackdrop.localPosition = new Vector3(0.5f, Math.Max(this.shieldBackdrop.localPosition.y - 0.05f, 0), 0);
    }
  }

  private void UpdateDamageBar() {
    float actual = this.damageBar.localScale.x;
    float desired = ShieldPercentage();

    if (desired < actual) {
      ResetRegenTimer();
      SetDamageBar(actual - 0.01f);

      if (this.damageBar.localScale.x < desired)
        SetDamageBar(desired);
    } else
      SetDamageBar(0);
  }

  private void RegenShield() {
    if (Stitch.Shield < 100 && !regenTimer.Enabled && Stitch.CurShield > 0) {
      Stitch.Shield += 1;
      SetShieldBar();
    }
  }

  private void ResetRegenTimer() {
    regenTimer.Enabled = false;
    regenTimer.Enabled = true;
  }

  private void SetShieldBar() {
    this.shieldBar.localScale = new Vector3(ShieldPercentage(), 1f, 1f);
  }

  private void SetDamageBar(float percent) {
    this.damageBar.localScale = new Vector3(percent, 1f, 1f);
  }

  private float ShieldPercentage() {
    return Stitch.Shield / 100f;
  }

  private void SetColor(Color c) {
    shieldFront.gameObject.GetComponent<SpriteRenderer>().color = c;
    shieldBar.gameObject.GetComponent<SpriteRenderer>().color = c;
  }

  private void ToggleElectricity() {
    electricity.SetActive(Stitch.CurShield > 0);
  }

  /***********************************
   * TIMER HANDLERS
   **********************************/

  protected void RegenTimerElapsed(object source, ElapsedEventArgs e) {
    regenTimer.Enabled = false;
  }

  protected void SlideTimerElapsed(object source, ElapsedEventArgs e) {
    slideTimer.Enabled = false;
  }

}
