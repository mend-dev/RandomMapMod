using MelonLoader;
using BTD_Mod_Helper;
using RandomMapMod;
using HarmonyLib;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Models.Profile;
using Il2CppAssets.Scripts.Unity.UI_New.Main;
using Il2CppAssets.Scripts.Unity.UI_New;

[assembly: MelonInfo(typeof(RandomMapMod.RandomMapMod), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace RandomMapMod;

public class RandomMapMod : BloonsTD6Mod {
    public override void OnApplicationStart() {
        ModHelper.Msg<RandomMapMod>("RandomMapMod loaded!");
    }

    static MapSaveDataModel saveModel;
    static bool rejoin;
    [HarmonyPatch(typeof(InGame), nameof(InGame.RoundEnd))]
    internal static class Leave {
        [HarmonyPostfix]
        private static void Postfix(InGame __instance, int completedRound, int highestCompletedRound) {
            ModHelper.Msg<RandomMapMod>(completedRound);
            if ((completedRound + 1) % 5 == 0) {
                var maps = new System.Collections.Generic.List<string> { "AnotherBrick", "Cargo", "Cornfield", "Geared", "HighFinance", "Mesa", "MidnightMansion", "OffTheCoast", "PatsPond", "Peninsula", "Spillway", "SunkenColumns", "Underground", "XFactor", "AlpineRun", "CandyFalls", "Carved", "Cubism", "EndOfTheRoad", "FourCircles", "Hedge", "InTheLoop", "Logs", "LotusIsland", "MiddleOfTheRoad", "OneTwoTree", "ParkPath", "Resort", "Scrapyard", "Skates", "TheCabin", "TownCentre", "TreeStump", "Tutorial", "WinterPark", "#ouch", "BloodyPuddles", "DarkCastle", "DarkDungeons", "FloodedValley", "Infernal", "MuddyPuddles", "Quad", "Ravine", "Sanctuary", "Workshop", "AdorasTemple", "Balance", "Bazaar", "Chutes", "CoveredGarden", "Cracked", "Downstream", "Encrypted", "FiringRange", "Haunted", "KartsNDarts", "MoonLanding", "Quarry", "QuietStreet", "Rake", "SpiceIslands", "SpringSpring", "Streambed", "FrozenOver", "Polyphemus", "BloonariusPrime" };
                var random = new System.Random();
                saveModel = __instance.CreateCurrentMapSave(highestCompletedRound, __instance.MapDataSaveId);
                rejoin = true;
                InGame.instance.Quit();
                InGameData.Editable.selectedMap = maps[random.Next(maps.Count)];
            }

        }
    }

    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.Open))]
    internal static class Rejoin {
        [HarmonyPostfix]
        private static void Postfix() {
            if (!rejoin) return;
            rejoin = false;
            UI.instance.LoadGame(null, null, saveModel);
        }
    }
}