using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DALib.BaseCode;


namespace DALib
{
    /// <summary>
    /// Data Access Layer for in-memory storage
    /// </summary>
    public class DA_Net : DAClass
    {
        protected List<Driver> DriversList;
        protected readonly List<Trip> TripsList;

        public DA_Net()
        {
            DriversList = new List<Driver>();
            TripsList = new List<Trip>();
        }


        public override int ProcessFile(IEnumerable<string> dataStr)
        {
            foreach (string s in dataStr)
            {
                //Process lines with Driver and Trip. Discard everything else
                string[] fieldsStr = s.Split(FIELD_SEPARATOR_CHAR);
                if (fieldsStr.Length > 1)
                {
                    switch (fieldsStr[0])
                    {
                        case "Driver":
                            Driver.Add(fieldsStr, DriversList);
                            break;
                        
                        case "Trip":
                            Trip.Add(fieldsStr, DriversList, TripsList);
                            break;
                        
                        default:
                            //Unrecognized command, do nothing
                            break;
                    }
                }
            }

            Driver.CalculateAndClean(DriversList, TripsList);
            return 0;
        }


        public override IEnumerable<DriversReport> GenerateReport()
        {
            DriversReport[] RetVal = new DriversReport[DriversList.Count];
            int c = 0;
            
            DriversList = DriversList.OrderByDescending(item => item.TotalMiles).ToList();
            foreach (Driver d in DriversList)
            {
                RetVal[c] = d.GetReportRow();
                c++;
            }
            
            return RetVal;
        }
    }
}
