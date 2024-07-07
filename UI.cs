using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EndPointManager
{
    public class UI
    {
        public static void RunEndPointManager()
        {
            int optionSelected = 0;
            BL bl = new BL();
            string consoleMsg = string.Empty;
            string serialNumber = string.Empty;
            int switchState;
            
            // Loop to always return to main menu
            while (optionSelected != 6)
            {
                consoleMsg = string.Empty;
                Console.ResetColor();

                Console.WriteLine("Welcome to End Point Manager!" + Environment.NewLine);
                Console.WriteLine("Input a number to select an action:" + Environment.NewLine);
                Console.WriteLine("1) Insert a new endpoint");
                Console.WriteLine("2) Edit an existing endpoint");
                Console.WriteLine("3) Delete an existing endpoint");
                Console.WriteLine("4) List all endpoints");
                Console.WriteLine("5) Find an End Point");
                Console.WriteLine("6) Exit" + Environment.NewLine);
                Console.WriteLine("7) Generate Mock List" + Environment.NewLine);
                Console.Write("Number: ");

                try
                {
                    optionSelected = int.Parse(Console.ReadLine());
                    Console.Clear();

                    switch (optionSelected)
                    {
                        case 1: // Insert
                            consoleMsg = Insert(ref bl);
                            break;

                        case 2: // update
                            consoleMsg = Edit(ref bl);
                            break;

                        case 3: // delete
                            consoleMsg = Delete(ref bl);
                            break;

                        case 4: // List all end points
                            consoleMsg = ListAll(ref bl);                           
                            break;
                        case 5: // Find an end point
                            consoleMsg = Find(ref bl);                           
                            break;

                        case 6: // Exit
                            // Confirming exit
                            if (Exit()== "Y")
                                continue;
                            else // Canceling the exit
                                optionSelected = 0;
                            break;

                        case 7:
                            consoleMsg = GenerateMockList(ref bl);
                            break;
                        default:
                            throw new Exception("Invalid option");
                    }

                    if (consoleMsg.Length > 0)
                        Console.WriteLine(Environment.NewLine + consoleMsg + Environment.NewLine);

                    Console.ResetColor();

                    Console.WriteLine("Press 'Enter' to return to Main Menu...");
                    var i = Console.ReadLine();
                    Console.Clear();

                }
                catch (Exception ex)
                {
                    // All validations were handled here
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(Environment.NewLine + ex.Message + Environment.NewLine);
                    Console.ResetColor();
                    Console.WriteLine("Press 'Enter' to return to Main Menu...");
                    var i = Console.ReadLine();
                    Console.Clear();

                }
            }
        }

        private static int getSwitchStateAnsware(bool cancel = false)
        {
            Console.WriteLine("1) Disconnected");
            Console.WriteLine("2) Connected");
            Console.WriteLine("3) Armed");
            if (cancel)
                Console.WriteLine("4) Cancel edit");

            Console.Write("Option: ");
            int switchState = int.Parse(Console.ReadLine()) - 1;

            Console.Clear();

            return switchState;

        }
        private static string Insert(ref BL bl)
        {
            string consoleMsg = string.Empty;
            Console.Clear();
            Console.Write("Inform Serial Number: ");
            string serialNumber = Console.ReadLine();
            // Could validate the serial right away, but to provide more data to handle the tests I prefer to
            // follow the flow of the console, additionaly I will need to do more loops in the console to await for 
            // a valid data
            Console.WriteLine("");
            Console.Write("Inform Meter Model ID: ");
            string modelId = Console.ReadLine();
            Console.WriteLine("");
            Console.Write("Inform Meter Number: ");
            int meterNumber = int.Parse(Console.ReadLine());
            Console.WriteLine("");
            Console.Write("Inform Meter Firmware Version: ");
            string firmwareVersion = Console.ReadLine();
            Console.WriteLine("");
            Console.WriteLine("Inform an option for Switch State");
            int switchState = getSwitchStateAnsware();

            if (bl.InsertEndPoint(serialNumber, modelId, meterNumber, firmwareVersion, switchState) > 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                consoleMsg = "End Point succesfully inserted";
            }

            return consoleMsg;
        }        
        private static string Edit(ref BL bl)
        {
            string consoleMsg = string.Empty;
            Console.Clear();
            Console.Write("Inform Serial Number: ");
            string serialNumber = Console.ReadLine();

            EndPoint edit = bl.FindEndPoint(serialNumber);

            Console.Clear();
            Console.WriteLine("Serial: " + edit.SerialNumber +
                              " , Switch State: " + ((States)edit.SwitchState).ToString() + Environment.NewLine);
            Console.WriteLine("Choose an option to edit the switch state:");
            int switchState = getSwitchStateAnsware(true);

            if (switchState != 4)
            {
                if (bl.EditEndPoint(serialNumber, switchState) > 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    consoleMsg = "End Point succesfully edited";
                }
            }

            return consoleMsg;
        }
        private static string Delete (ref BL bl)
        {
            string consoleMsg = string.Empty;
            Console.Clear();
            Console.Write("Inform Serial Number: ");
            string serialNumber = Console.ReadLine();

            EndPoint delete = bl.FindEndPoint(serialNumber);

            Console.Clear();

            string deleting = string.Empty;
            // Loop to await a valid confirmation (Y/N)
            while (true)
            {
                Console.Clear();
                Console.WriteLine(Environment.NewLine +
                                  "Are you sure you want to delete this End Point? " +
                                  Environment.NewLine);
                printEndPoint(delete);
                Console.WriteLine("");
                Console.Write("Y/N :");
                deleting = Console.ReadLine();

                if (deleting.ToUpper() == "Y" || deleting.ToUpper() == "N")
                    break;
            }

            if (deleting.ToUpper() == "Y" && bl.DeleteEndPoint(serialNumber) > 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                consoleMsg = "End Point succesfully deleted";

            }

            return consoleMsg;
        }
        private static string ListAll (ref BL bl)
        {
            List<EndPoint> endPoints = bl.getAllEndPoints();
            foreach (var endPoint in endPoints)
            {
                printEndPoint(endPoint);
            }

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            return "Founded " + endPoints.Count.ToString() + " End Point(s)";
        }
        private static string Find (ref BL bl)
        {
            Console.Clear();
            Console.Write("Inform Serial Number: ");
            string serialNumber = Console.ReadLine();

            EndPoint find = bl.FindEndPoint(serialNumber);

            Console.Clear();
            Console.WriteLine("");
            printEndPoint(find);
            Console.WriteLine("");
            return string.Empty;
        }
        private static string Exit ()
        {
            string exiting = string.Empty;
            // Loop to await a valid confirmation (Y/N)
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Are you sure you want to exit? Y/N");
                exiting = Console.ReadLine();

                if (exiting.ToUpper() == "Y" || exiting.ToUpper() == "N")
                    break;
            }
            return exiting.ToUpper();
        }
        private static string GenerateMockList(ref BL bl)
        {
            bl.GenerateMockList();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            return "List inserted successfully";
        }

        private static void printEndPoint(EndPoint endPoint)
        {
            Console.WriteLine("Serial: " + endPoint.SerialNumber +
                                  ", Model: " + ((Models)endPoint.MeterModelId).ToString() +
                                  ", Meter Number: " + endPoint.MeterNumber +
                                  ", Firmware Version: " + endPoint.MeterFirmwareVersion +
                                  ", Switch State: " + ((States)endPoint.SwitchState).ToString());
        }
    }
}
