using UnityEngine;

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

  /***********************************
   * GAME GLOBALS
   **********************************/

  void Awake() {
    instance = this;
    player = GameObject.FindWithTag("Player").GetComponent<Player_Base>();
  }

  // Treat this class as a singleton. This will hold the instance of the class.
  private static Stitch instance;
  public static Stitch Instance { get { return instance; } }

  private Player_Base player;
  public Player_Base LocalPlayer { get { return Player; } }
  public static Player_Base Player { get { return Stitch.Instance.LocalPlayer; } }

  private static int shield = 100;
  public static int Shield { get { return shield; } set { shield = value; } }

  private static int maxShield = 3;
  public static int MaxShield { get { return maxShield; } }

  private static int curShield = 2;
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
}
