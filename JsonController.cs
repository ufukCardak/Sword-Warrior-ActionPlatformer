using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonController : MonoBehaviour
{
    public PlayerAssignments player = new PlayerAssignments();
    string path;
    private void Awake()
    {
        path = Path.Combine(Application.persistentDataPath, "PlayerJson.json");
        if (!File.Exists(path))
        {
            Debug.Log("2");
            player.colorHexCode = "";
            player.currentExp = 0;
            player.expSliderMax = 100;
            player.currentLevel = 1;
            player.currentWeaponUpgrade = 1;
            player.currentPlayerUpgrade = 1;
            player.currentCoin = 0;
            player.weaponReqLevel = 0;
            player.playerReqLevel = 0;
            player.weaponPriceMultiplier = 1;
            player.playerPriceMultiplier = 1;
            player.glowPower = 1;
            player.damage = 0;
            player.healthSliderMax = 100;
            JsonSave();
        }
        //JsonReset();
        JsonLoad();
    }
    void JsonReset()
    {
        player.colorHexCode = "";
        player.currentExp = 0;
        player.expSliderMax = 100;
        player.currentLevel = 1;
        player.currentWeaponUpgrade = 1;
        player.currentPlayerUpgrade = 1;
        player.currentCoin = 0;
        player.weaponReqLevel = 0;
        player.playerReqLevel = 0;
        player.weaponPriceMultiplier = 1;
        player.playerPriceMultiplier = 1;
        player.glowPower = 1;
        player.damage = 0;
        player.healthSliderMax = 100;
        JsonSave();
    }
    public void JsonSave()
    {
        string jsonString = JsonUtility.ToJson(player,true);

        File.WriteAllText(path, jsonString);
    }
    public void JsonLoad()
    {
        if (File.Exists(path))
        {
            string readJson = File.ReadAllText(path);
            player = JsonUtility.FromJson<PlayerAssignments>(readJson);
        }
    }
}
