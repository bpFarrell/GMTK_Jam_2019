﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Room))]
public class RoomEditor : Editor {
    public override void OnInspectorGUI()
    {
        Room script = (Room) target;


        GUILayout.BeginHorizontal();
        
        GUILayout.BeginVertical();
            
        if (GUILayout.Button("NorthWest")) {
            script.CreateNeighbor(COMPASS_DIR.NORTHWEST);
        }
        EditorGUILayout.ObjectField( script.rooms.nw, typeof(Room), true);
        GUILayout.Space(40f);
        if (GUILayout.Button("SouthWest")) {
            script.CreateNeighbor(COMPASS_DIR.SOUTHWEST);
        }
        EditorGUILayout.ObjectField( script.rooms.nw, typeof(Room), true);

        GUILayout.EndVertical();
        GUILayout.Space(80f);
            
        GUILayout.BeginVertical();
            
        if (GUILayout.Button("NorthEast")) {
            script.CreateNeighbor(COMPASS_DIR.NORTHEAST);
        }
        EditorGUILayout.ObjectField( script.rooms.nw, typeof(Room), true);
        GUILayout.Space(40f);
        if (GUILayout.Button("SouthEast"))
        {
            script.CreateNeighbor(COMPASS_DIR.SOUTHEAST);
        }
        EditorGUILayout.ObjectField( script.rooms.nw, typeof(Room), true);

        GUILayout.EndVertical();

    
        GUILayout.EndHorizontal();
    }
}