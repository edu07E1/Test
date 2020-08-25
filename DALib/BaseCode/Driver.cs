using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DALib.BaseCode
{
    public class Driver
    {
        private string DriverName;
        public int ValidFlag;

        private int AvgSpeed;
        public int TotalMiles { get; set; }


        public Driver(string pDriverName, int pValidFlag)
        {
            DriverName = pDriverName;
            ValidFlag = pValidFlag;
        }


        public DriversReport GetReportRow()
        {
            if (TotalMiles > 0)
                return new DriversReport($"{DriverName}: {TotalMiles} @ {AvgSpeed} mph");
            else
                return new DriversReport($"{DriverName}: {TotalMiles}");
        }


        /// <summary>
        /// Add Driver with ValidFlag=1
        /// </summary>
        /// <param name="s">"Driver" DriverName</param>
        /// <param name="DriverList"></param>
        public static void Add(string[] s, List<Driver> DriverList)
        {
            //Validations
            //Example: Driver Dan
            if(s.Length==2)
            {
                if(s[0]=="Driver" && s[1].Length > 0)
                {
                    //Valid data, add to list if not already exists
                    Driver d = DriverList.FirstOrDefault(m => m.DriverName == s[1]);
                    if (d == null)
                        DriverList.Add(new Driver(s[1], 1));
                    else
                        if (d.ValidFlag == 0)
                            d.ValidFlag = 1;
                }
            }
        }


        /// <summary>
        /// Add Driver with ValidFlag=0
        /// ValidFlag=0 means Driver and Trip will be deleted if Driver command for this driver is not found in file
        /// </summary>
        /// <param name="pDriverName"></param>
        /// <param name="DriverList"></param>
        public static void Add(string pDriverName, List<Driver> DriverList)
        {
            //Validations
            if (pDriverName.Length > 0)
            {
                //Valid data, add to list if not already exists
                Driver d = DriverList.FirstOrDefault(m => m.DriverName == pDriverName);
                if (d == null)
                    DriverList.Add(new Driver(pDriverName, 0));
            }
        }


        public static void CalculateAndClean(List<Driver> DriverList, List<Trip> TripList)
        {
            foreach (Driver d in DriverList)
            {
                if(d.ValidFlag==0)
                {
                    //Delete Driver and Trips
                    TripList.RemoveAll(m => m.DriverName == d.DriverName);
                    DriverList.Remove(d);
                }
                else
                {
                    //Calculate
                    d.TotalMiles = TripList.Where(m => m.DriverName == d.DriverName).DefaultIfEmpty().Sum(m => m==null?0 :m.Distance);
                    //d.AvgSpeed = (int)Math.Round(TripList.Where(m => m.DriverName == d.DriverName).DefaultIfEmpty().Average(m => m==null?0 :m.Speed));
                    double TotalTime = TripList.Where(m => m.DriverName == d.DriverName).DefaultIfEmpty().Sum(m => m==null?0 : m.TripTimeInHours);
                    d.AvgSpeed = TotalTime == 0 ? 0 : (int)Math.Round(d.TotalMiles / TotalTime);
                }
            }
        }
    }
}
