using System;
using System.Collections.Generic;
using MSCLoader;
using OASIS;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MSCElectricAPI
{
    public static class Manager
    {
        internal static List<GameObject> SocketPrefabs = new List<GameObject>();
        internal static List<GameObject> PlugPrefabs = new List<GameObject>();
        internal static Collider[] sockets = new Collider[10000];
        internal static int socketCount;

        public static void Init()
        {

        }

        public static void CreateSocket(Vector3 position, Vector3 rotation, SocketType type,
            PartColor color = PartColor.White, string name = "")
        {
            var socket = (GameObject)Object.Instantiate(ChooseModel(type, color), position, Quaternion.Euler(rotation));
            var colliders = socket.GetComponentsInChildren<Collider>();
            var socketsLength = sockets.Length;

            foreach (var collider in colliders)
            {
                if (socketCount < socketsLength)
                {
                    sockets[socketCount++] = collider;
                }
                else
                {
                    ModConsole.Warning("Socket array is full. Cannot add more sockets.");
                    break;
                }
            }

            socket.name = !string.IsNullOrEmpty(name) ? name : $"Socket {socketCount - colliders.Length}";
        }

        private static GameObject ChooseModel(SocketType type, PartColor color)
        {
            var modelName = type != SocketType.Vintage
                ? $"Socket{type}{(color == PartColor.White ? "" : color.ToString())}"
                : "SocketVintage";

            foreach (var prefab in SocketPrefabs)
            {
                if (prefab.name == modelName)
                {
                    return prefab;
                }
            }

            throw new InvalidOperationException($"Model {modelName} not found in SocketPrefabs.");
        }

        private static GameObject ChooseModel(PlugType type, PartColor color)
        {
            var modelName = $"Plug{type}{(color == PartColor.White ? "" : color.ToString())}";

            foreach (var prefab in PlugPrefabs)
            {
                if (prefab.name == modelName)
                {
                    return prefab;
                }
            }

            throw new InvalidOperationException($"Model {modelName} not found in PlugPrefabs.");
        }

        public static void CreatePlug(Vector3 position, Vector3 rotation, PlugType type,
            PartColor color = PartColor.White, GameObject cable = null, string name = "")
        {
            var plug = (GameObject)Object.Instantiate(ChooseModel(type, color), position, Quaternion.Euler(rotation));
            plug.name = (!string.IsNullOrEmpty(name) ? name : "Plug") + "(Clone)";
            plug.MakePickable();

            var part = plug.AddComponent<JointedPart>();
            part.breakForce = 1000;
            part.triggers = sockets;
            part.disableSound = true;
            part.onAttach = i =>
                MasterAudio.PlaySound3DAndForget("HouseFoley", part.transform, variationName: "light_switch");
            part.onBreak = (i, f) => MasterAudio.PlaySound3DAndForget("HouseFoley", volumePercentage: 0.5f,
                sourceTrans: part.transform, variationName: "light_switch");
        }
    }
}