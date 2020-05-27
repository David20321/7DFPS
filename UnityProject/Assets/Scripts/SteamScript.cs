﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using ImGuiNET;

public class SteamScript : MonoBehaviour
{
    string steamName;

    // Start is called before the first frame update
    void Start()
    {
        if (SteamManager.Initialized) {
            steamName = SteamFriends.GetPersonaName();
        }
    }

    // Update is called once per frame
    void Update()
    {
        ImGui.Begin("Steam Test Window");

        ImGui.Text("Steam user name: " + steamName);
        ImGui.Dummy(new Vector2(0.0f, 10.0f));

        ImGui.Text("Available mods:");
        foreach (Mod mod in ModManager.availableMods) {
            ImGui.Text(mod.name);
            ImGui.SameLine(120);
            if (ImGui.Button("Upload to Steam Workshop")) {
                // todo
            }
        }

        ImGui.End();
    }
}
