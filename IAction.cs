using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndPointManager
{
    public interface IAction
    {
        int InsertEndPoint(string serialNumber, string meterModelId, int meterNumber, string meterFirmwareVersion, int switchState) => throw new NotImplementedException();
        int EditEndPoint(string serialNumber, int newState) => throw new NotImplementedException();
        int DeleteEndPoint(string serialNumber) => throw new NotImplementedException();
        List<EndPoint> getAllEndPoints() => throw new NotImplementedException();
    }
}
