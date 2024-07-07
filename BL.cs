using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EndPointManager
{

    public class BL : IAction
    {
        // Acting as data repository
        private List<EndPoint> _endPoints;

        public BL()
        {
            _endPoints = new List<EndPoint>();
        }
        public void GenerateMockList()
        {
            // Data to fast generate, using insert function to validate duplicated items
            InsertEndPoint("SN00000001", ((Models)16).ToString(), 2196, "V 1.0.9.53", 1);
            InsertEndPoint("SN00000004", ((Models)18).ToString(), 4093, "V 1.0.9.53", 2);
            InsertEndPoint("SN00000007", ((Models)16).ToString(), 2552, "V 1.0.9.53", 2);
            InsertEndPoint("SN00000010", ((Models)16).ToString(), 4796, "V 1.0.9.53", 2);
            InsertEndPoint("SN00000013", ((Models)19).ToString(), 3001, "V 1.0.9.53", 1);
            InsertEndPoint("SN00000016", ((Models)18).ToString(), 4650, "V 1.0.9.53", 0);
            InsertEndPoint("SN00000019", ((Models)16).ToString(), 4653, "V 1.0.9.53", 2);
            InsertEndPoint("SN00000022", ((Models)16).ToString(), 3577, "V 1.0.9.53", 2);
            InsertEndPoint("SN00000025", ((Models)16).ToString(), 1213, "V 1.0.9.53", 2);
            InsertEndPoint("SN00000028", ((Models)16).ToString(), 1975, "V 1.0.9.53", 2);
            InsertEndPoint("SN00000031", ((Models)18).ToString(), 2194, "V 1.0.9.53", 2);
            InsertEndPoint("SN00000034", ((Models)17).ToString(), 2459, "V 1.0.9.53", 2);
            InsertEndPoint("SN00000037", ((Models)16).ToString(), 3124, "V 1.0.9.53", 2);
            InsertEndPoint("SN00000040", ((Models)16).ToString(), 1320, "V 1.0.9.53", 1);
            InsertEndPoint("SN00000043", ((Models)17).ToString(), 2601, "V 1.0.9.53", 2);
            InsertEndPoint("SN00000046", ((Models)16).ToString(), 2267, "V 1.0.9.53", 2);
            InsertEndPoint("SN00000049", ((Models)18).ToString(), 1827, "V 1.0.9.53", 1);
            InsertEndPoint("SN00000052", ((Models)18).ToString(), 4092, "V 1.0.9.53", 0);
            InsertEndPoint("SN00000055", ((Models)17).ToString(), 2773, "V 1.0.9.53", 1);
            InsertEndPoint("SN00000058", ((Models)19).ToString(), 3190, "V 1.0.9.53", 1);
            InsertEndPoint("SN00000061", ((Models)17).ToString(), 991, "V 1.0.9.53", 1);
            InsertEndPoint("SN00000064", ((Models)19).ToString(), 3211, "V 1.0.9.53", 2);

        }
        public int InsertEndPoint(string serialNumber, string meterModelId, int meterNumber, string meterFirmwareVersion, int switchState)
        {
            // Validate serial number
            if (string.IsNullOrEmpty(serialNumber))
                throw new Exception("Serial Number not informed");
            if (serialNumber.Length > 20)
                throw new Exception("Serial Number too long (Max 20 characters)");

            // Validate if Model is valid
            Enum.TryParse(meterModelId, out Models modelId);
            if (!Enum.IsDefined(typeof(Models), modelId))
                throw new Exception("Model invalid");

            // Validate negative number for meter number
            if (meterNumber < 0)
                throw new Exception("Meter Number cannot be negative");

            // Validate firmware version
            if (string.IsNullOrEmpty(meterFirmwareVersion))
                throw new Exception("Firmware Version not informed");
            if (meterFirmwareVersion.Length > 20)
                throw new Exception("Firmware Version too long (Max 20 characters)");

            // Validate Switch State
            if (!Enum.IsDefined(typeof(States), switchState))
                throw new Exception("Invalid Switch State");

            // Validate Existing Serial Number
            if (FindEndPoint(serialNumber, true) != null)
                throw new Exception("Serial Number already in use");
            
            _endPoints.Add(new EndPoint(serialNumber, modelId, meterNumber, meterFirmwareVersion, switchState));

            return 1;
        }
        // Utilizing same function to check for existing serial number and to retrieve an item
        public EndPoint FindEndPoint(string serialNumber, bool checking = false)
        {
            EndPoint endPoint = _endPoints.Where(w => w.SerialNumber == serialNumber).FirstOrDefault();

            if (endPoint == null && checking == false)
                throw new Exception("Serial Number not found");

            return endPoint;
        }

        public int EditEndPoint(string serialNumber, int newState)
        {

            int index = _endPoints.IndexOf(FindEndPoint(serialNumber));
            if (!Enum.IsDefined(typeof(States), newState))
                throw new Exception("Invalid Switch State");

            if (index == -1)
                throw new Exception("Fail to edit Switch State.");
            else
                _endPoints[index].SwitchState = newState;

            return 1;
        }

        public int DeleteEndPoint(string serialNumber)
        {
            EndPoint endPoint = FindEndPoint(serialNumber);
            _endPoints.Remove(endPoint);

            return 1;
        }

        public List<EndPoint> getAllEndPoints()
        {
            return _endPoints;
        }

    }
}
