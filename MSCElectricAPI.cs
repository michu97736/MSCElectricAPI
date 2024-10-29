using MSCLoader;
using UnityEngine;

namespace MSCElectricAPI
{
    public class MSCElectricAPI : Mod
    {
        public override string ID => "MSCElectricAPI"; // Your (unique) mod ID 
        public override string Name => "MSCElectricAPI"; // Your mod name
        public override string Author => "michu97736"; // Name of the Author (your name)
        public override string Version => "1.0"; // Version
        public override string Description => "Adds electric sockets."; // Short description of your mod

        public override void ModSetup()
        {
            SetupFunction(Setup.OnLoad, Mod_OnLoad);
            SetupFunction(Setup.ModSettings, Mod_Settings);
        }

        private void Mod_Settings()
        {
            // All settings should be created here. 
            // DO NOT put anything that isn't settings or keybinds in here!
        }

        private void Mod_OnLoad()
        {
            AssetBundle bundle = AssetBundle.CreateFromMemoryImmediate(Properties.Resources.sockets);
            foreach (string name in bundle.GetAllAssetNames())
            {
                if (!name.EndsWith(".prefab"))
                    continue;
                GameObject obj = bundle.LoadAsset(name) as GameObject;
                if (obj.name.Contains("Socket"))
                    Manager.SocketPrefabs.Add(obj);
                else if (obj.name.Contains("Plug"))
                    Manager.PlugPrefabs.Add(obj);
            }
            Manager.Init();
            Manager.CreateSocket(new Vector3(0, 0, 0), new Vector3(0, 0, 0), SocketType.Normal);
            Manager.CreateSocket(new Vector3(2, 0, 0), new Vector3(0, 0, 0), SocketType.Normal, PartColor.Black);
            Manager.CreateSocket(new Vector3(4, 0, 0), new Vector3(0, 0, 0), SocketType.Double);
            Manager.CreateSocket(new Vector3(6, 0, 0), new Vector3(0, 0, 0), SocketType.Double, PartColor.Black);
            Manager.CreateSocket(new Vector3(8, 0, 0), new Vector3(0, 0, 0), SocketType.Vintage);
            Manager.CreatePlug(new Vector3(10, 0, 0), new Vector3(0, 0, 0), PlugType.Normal);
            Manager.CreatePlug(new Vector3(12, 0, 0), new Vector3(0, 0, 0), PlugType.Euro);

        }
    }
}
