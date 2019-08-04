using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Room))]
public class RoomEditor : Editor {
    public override void OnInspectorGUI()
    {
        Room script = (Room) target;

        script.torchContainer = EditorGUILayout.ObjectField("Torch Container", script.torchContainer, typeof(Transform), true) as Transform;
        script.doorContainer = EditorGUILayout.ObjectField("Door Container", script.doorContainer, typeof(Transform), true) as Transform;
        script.floorBounds = EditorGUILayout.ObjectField("Floor Reference", script.floorBounds, typeof(MeshRenderer), true) as MeshRenderer;

        script.cameraOverride = EditorGUILayout.FloatField("Camera Size Override", script.cameraOverride);

        GUILayout.Space(20f);
        GUILayout.BeginHorizontal();
        
        GUILayout.BeginVertical();
            
        if (GUILayout.Button("NorthWest")) {
            script.CreateNeighbor(COMPASS_DIR.NORTHWEST);
        }
        script.rooms.nw = EditorGUILayout.ObjectField( script.rooms.nw, typeof(Room), true) as Room;
        GUILayout.Space(40f);
        if (GUILayout.Button("SouthWest")) {
            script.CreateNeighbor(COMPASS_DIR.SOUTHWEST);
        }
        script.rooms.sw = EditorGUILayout.ObjectField( script.rooms.sw, typeof(Room), true) as Room;

        GUILayout.EndVertical();
        GUILayout.Space(80f);
        GUILayout.BeginVertical();
            
        if (GUILayout.Button("NorthEast")) {
            script.CreateNeighbor(COMPASS_DIR.NORTHEAST);
        }
        script.rooms.ne = EditorGUILayout.ObjectField( script.rooms.ne, typeof(Room), true) as Room;
        GUILayout.Space(40f);
        if (GUILayout.Button("SouthEast"))
        {
            script.CreateNeighbor(COMPASS_DIR.SOUTHEAST);
        }
        script.rooms.se = EditorGUILayout.ObjectField( script.rooms.se, typeof(Room), true) as Room;

        GUILayout.EndVertical();
    
        GUILayout.EndHorizontal();
    }
}
