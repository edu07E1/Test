using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;


namespace DALib.BaseCode
{
    public class Trip
    {
        public string DriverName { get; set; }
        public TimeSpan StartTime;
        public TimeSpan EndTime;
        public int Distance { get; set; }
        public double Speed { get; set; }
        public double TripTimeInHours { get => (EndTime - StartTime).TotalHours; }


        public Trip(string pDriverName, TimeSpan pStartTime, TimeSpan pEndTime, int pDistance, double pSpeed)
        {
            DriverName = pDriverName;
            StartTime = pStartTime;
            EndTime = pEndTime;
            Distance = pDistance;
            Speed = pSpeed;
        }


        /// <summary>
        /// Verify if times overlap with existing time data
        /// </summary>
        /// <param name="pStartTime"></param>
        /// <param name="pEndTime"></param>
        /// <returns>false: Times are valid, don't overlap, true: Overlap</returns>
        public bool CheckTimes(TimeSpan pStartTime, TimeSpan pEndTime)
        {
            return (pStartTime >= StartTime && pStartTime <= EndTime) ||
                   (pEndTime >= StartTime && pEndTime <= EndTime);
        }


        public static void Add(string[] s, List<Driver> DriverList, List<Trip> TripList)
        {
            double Distance, Speed;
            int DistanceInt;
            TimeSpan StartTime, EndTime;

            //Validations
            //Example: Trip Dan 07:15 07:45 17.3
            if (s.Length == 5)
            {
                if (s[0] == "Trip" && s[1].Length > 0 && s[2].Length==5 && s[3].Length==5 && s[4].Length > 0)
                {
                    StartTime = TimeSpan.ParseExact(s[2], "hh\\:mm", CultureInfo.InvariantCulture);
                    EndTime = TimeSpan.ParseExact(s[3], "hh\\:mm", CultureInfo.InvariantCulture);
                   
                    if (Double.TryParse(s[4], out Distance))
                    {
                        DistanceInt = (int)Math.Round(Distance);
                        if (EndTime > StartTime && Distance > 0)
                        {
                            //Valid data, check if times don't overlap with existing data
                            Trip d = TripList.FirstOrDefault(m => m.CheckTimes(StartTime, EndTime));
                            if (d == null)
                            {
                                Speed = Distance / (EndTime - StartTime).TotalHours;
                                if (Speed >= 5 && Speed <= 100)
                                {
                                    //Data OK, add Trip
                                    TripList.Add(new Trip(s[1], StartTime, EndTime, DistanceInt, Speed));

                                    //Add driver if doesn't exists
                                    //ValidFlag=0 means Driver and Trip will be deleted if Driver command for this driver is not found in file
                                    Driver.Add(s[1], DriverList);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
