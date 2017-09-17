using UnityEngine;
using System;
using System.Timers;

public class Stitch : MonoBehaviour
{
  /***********************************
   * DRAWING CONSTANTS
   **********************************/

  public static bool SHOW_GAME_GRID = false;
  public static bool SHOW_OBJ_ORIGIN = true;
  public static bool SHOW_MASK_BOUNDS = true;
  public static bool SHOW_TERRAIN_BOUNDS = true;
  public static bool SHOW_WATER_BOUNDS = true;
  public static bool SHOW_RAIL_PATHS = true;
  public static bool SHOW_CAMERA_BOUNDS = true;
  public static bool SHOW_EXIT_BOUNDS = true;
  public static bool SHOW_ENTRANCES = true;

  private Player_Base player;

  /***********************************
   * PRIVATE WEAPONS MENU VARS
   **********************************/

  private Transform weaponsMenu;
  private Transform[] weapons;
  private float weaponsCircleRadius = 0f;

  private int activeWeaponIndex = -1;
  private int ActiveWeaponIndex {
    get { return activeWeaponIndex; }
    set {
      // Set a timer for diagonal weapons.
      // Prevents the accidental selection of non-diagonals when releasing the button.
      if ((value % 2) == 1) {
        lockPos[0] = value == 0 ? 7 : value - 1;
        lockPos[1] = value == 7 ? 0 : value + 1;
        weaponChangeTimer.Enabled = false;
        weaponChangeTimer.Enabled = true;
        activeWeaponIndex = value;
      } else if (!weaponChangeTimer.Enabled || (lockPos[0] != value && lockPos[1] != value)) {
        weaponChangeTimer.Enabled = false;
        activeWeaponIndex = value;
      }
    }
  }

  private int[] lockPos = new int[2]; // Indexes of positions to lock when then weapons change timer is active.
  private Timer weaponChangeTimer = new Timer();

  /***********************************
   * GAME GLOBALS
   **********************************/

  void Awake() {
    instance = this;

    weapons = new Transform[8];
    this.weaponsMenu = GameObject.Find("WeaponsMenu").transform;
    int i = 0;
    foreach (Transform t in weaponsMenu.transform)
      weapons[i++] = t;

    weaponChangeTimer.Elapsed += new ElapsedEventHandler(UnlockWeaponWheel);
    weaponChangeTimer.Interval = 250;

    player = GameObject.Find("Player").GetComponent<Player_Base>();
  }

  void Update() {
    // Show weapon menu
    if (Game.TresHeld) {
      if (!Stitch.showWeaponsMenu && !Game.paused) {
        Stitch.showWeaponsMenu = true;
        Game.paused = true;
        Game.pausedByPlayer = false;
      }
    } else {
      if (Stitch.showWeaponsMenu) {
        Stitch.showWeaponsMenu = false;
        Game.paused = false;
      }
    }

    UpdateActiveWeaponIndex();
    ToggleWeaponsMenu();
  }

  private string GetCurrentGun() {
    switch (ActiveWeaponIndex) {
      case 0:
        return "laser";
      case 2:
        return "steamer";
      case 4:
        return "launcher";
      case 6:
        return "scythe";
      default:
        return "pistol";
    }
  }

  private void UpdateActiveWeaponIndex() {
    if (Stitch.showWeaponsMenu) {
      if (Game.LeftHeld && Game.UpHeld)
        ActiveWeaponIndex = 3;
      else if (Game.RightHeld && Game.UpHeld)
        ActiveWeaponIndex = 1;
      else if (Game.LeftHeld && Game.DownHeld)
        ActiveWeaponIndex = 5;
      else if (Game.RightHeld && Game.DownHeld)
        ActiveWeaponIndex = 7;
      else if (Game.RightHeld)
        ActiveWeaponIndex = 0;
      else if (Game.UpHeld)
        ActiveWeaponIndex = 2;
      else if (Game.LeftHeld)
        ActiveWeaponIndex = 4;
      else if (Game.DownHeld)
        ActiveWeaponIndex = 6;

      if (ActiveWeaponIndex != -1) {
        player.CurrentGun = GetCurrentGun();
        player.Sprite.Step();
      }
    }
  }

  private void ToggleWeaponsMenu() {
    int i = 0;
    foreach (Transform weapon in weapons) {
      if (weapon == null)
        continue;

      SpriteRenderer s = weapon.GetComponent<SpriteRenderer>();

      if (Stitch.showWeaponsMenu)
        s.color = new Color(1, 1, 1, Math.Min(s.color.a + 0.1f, 1f));
      else
        s.color = new Color(1, 1, 1, Math.Max(s.color.a - 0.1f, 0f));

      weaponsCircleRadius = s.color.a;
      UpdateWeaponDepth(weapon, -s.color.a, i++);
    }

    UpdateWeaponsCircle();
  }

  private void UpdateWeaponDepth(Transform weapon, float baseDepth, int i) {
    float depth = 8f;
    float speed = 0.02f;

    if (ActiveWeaponIndex != -1) {
      if (i == ActiveWeaponIndex) {
        depth = 2.5f;
        speed = 0.04f;
      }
      else if (Math.Abs(i - ActiveWeaponIndex) == 1 || (ActiveWeaponIndex == 0 && i == 7) || (ActiveWeaponIndex == 7 && i == 0))
        depth = 4f;
    }

    float desiredDepth = baseDepth / depth;

    if (weapon.position.z > desiredDepth)
      weapon.position = new Vector3(weapon.position.x, weapon.position.y, Math.Max(weapon.position.z - speed, desiredDepth));
    else if (weapon.position.z < desiredDepth)
      weapon.position = new Vector3(weapon.position.x, weapon.position.y, Math.Min(weapon.position.z + speed, desiredDepth));
  }

  private void UpdateWeaponsCircle() {
    float centerX = transform.position.x;
    float centerY = transform.position.y;

    int i = 0;
    float angle = 0f;
    foreach (Transform weapon in weapons) {
      float xDivide = (i == 1 || i == 3 || i == 5 || i == 7) ? 1.15f : 1f;
      weapon.localPosition = new Vector3((centerX + weaponsCircleRadius * (float)Math.Cos(angle)) / xDivide, (centerY + weaponsCircleRadius * (float)Math.Sin(angle)) / 1.55f, weapon.localPosition.z);
      angle += (float)(Math.PI / 4f);
      i++;
    }
  }

  protected void UnlockWeaponWheel(object source, ElapsedEventArgs e) {
    weaponChangeTimer.Enabled = false;
  }

  // Treat this class as a singleton. This will hold the instance of the class.
  private static Stitch instance;
  public static Stitch Instance { get { return instance; } }

  private static int shield = 100;
  public static int Shield { get { return shield; } set { shield = value; } }

  private static int maxShield = 3;
  public static int MaxShield { get { return maxShield; } }

  private static int curShield = 1;
  public static int CurShield { get { return curShield; } set { curShield = value; } }
  public static Color CurShieldColor {
    get {
      switch (curShield) {
        case 1:
          return Color.magenta;
        case 2:
          return Color.cyan;
        case 3:
          return Color.green;
        default:
          return Color.red;
      }
    }
  }

  private static bool showWeaponsMenu = false;
}
