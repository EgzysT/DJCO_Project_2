using System.Diagnostics;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WorldChanger))]
public class WorldChangerEditor : Editor {

    public override void OnInspectorGUI() {

        DrawDefaultInspector();

        if (!GUI.changed) {
            return;
        }
        
        WorldChanger worldChanger = target as WorldChanger;
        Renderer rend = null;
        worldChanger.TryGetComponent(out rend);

        if (rend != null) {
            GUILayout.Label(worldChanger.belongsTo.ToString());
            switch(worldChanger.belongsTo) {
                case World.NORMAL:
                    rend.material = new Material(Shader.Find("Custom/DisappearShader"));
                    break;
                case World.ARCANE:
                    rend.material = new Material(Shader.Find("Custom/AppearShader"));
                    break;
                case World.BOTH:
                    rend.material = new Material(Shader.Find("Custom/SwitchShader"));
                    break;
            }
        }
    }
}