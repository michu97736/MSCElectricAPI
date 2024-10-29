using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OASIS;
using UnityEngine;

namespace MSCElectricAPI
{
    public enum SocketType
    {
        Normal,
        Double,
        Vintage
    }

    public enum PartColor
    {
        White,
        Black
    }

    public enum PlugType
    {
        Normal,
        Euro
    }

    public class Socket : MonoBehaviour
    {
        public ElectricAppliance connectedAppliance;
    }

    public class Room : MonoBehaviour
    {
        public List<Socket> sockets = new List<Socket>();
    }

    public class ElectricAppliance : MonoBehaviour
    {
        private GameObject _plug;

        public GameObject Plug
        {
            get => _plug;
            set
            {
                _plug = value;
                if (_plug.GetComponent<JointedPart>().attachedTo != -1)
                {
                    currentSocket = Manager.sockets[_plug.GetComponent<JointedPart>().attachedTo];
                }
                else
                {
                    currentSocket = null;
                }
            }
        }

        public Collider currentSocket
        {
            get;
            private set;
        }

        public bool IsPowered => currentSocket != null;
    }
}
