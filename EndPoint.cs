using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndPointManager
{
    public enum Models
    {
        NSX1P2W = 16,
        NSX1P3W,
        NSX2P3W,
        NSX2P4W
    }
    public enum States
    {
        Disconnected = 0,
        Connected,
        Armed
    }
    public class EndPoint
    {
        public string SerialNumber { get; private set; }
        public int MeterModelId { get; private set; }
        public int MeterNumber { get; private set; }
        public string MeterFirmwareVersion { get; private set; }
        public int SwitchState { get; set; }

        public EndPoint (string serialNumber, Models meterModelId, int meterNumber, string meterFirmwareVersion, int switchState)
        {
            SerialNumber = serialNumber;
            MeterModelId = (int)meterModelId;
            MeterNumber = meterNumber;
            MeterFirmwareVersion = meterFirmwareVersion;
            SwitchState = switchState;
        }
    }
}
