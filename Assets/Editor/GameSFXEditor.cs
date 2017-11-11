using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameSFX))]
public class GameSFXEditor : Editor {
  public override void OnInspectorGUI() {
    DrawDefaultInspector();

    GameSFX sfx = (GameSFX)target;
    if (GUILayout.Button("Load Scene SFX")) {
      sfx.Editor_LoadSceneSFX();
    }
  }
}
