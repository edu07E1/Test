using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DALib;
using DALib.BaseCode;

namespace TripsCore.Code
{
    public class DataFile
    {
        public enum LFStatus { OK = 0, InvalidFileName = -1, LoadError = -2, InvalidParameters = -3 }
        
        private readonly int maxLineLen;
        private readonly IFormFile file;
        private StreamReader sr;
        private DAClass dA;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMaxLineLen">Maximum number of characters per line, not counting CR-LF (it's a security limit)</param>
        public DataFile (IFormFile pFile, int pMaxLineLen)
        {
            file = pFile;
            maxLineLen = pMaxLineLen;
        }


        /// <summary>
        /// Process content of .dat or .txt file
        /// </summary>
        /// <param name="DAType">Data Acces Type: 1: .NET, 2: SQL Server</param>
        /// <returns>LFStatus</returns>
        public LFStatus LoadFile (int DAType)
        {
            LFStatus retVal;
           
            //Create Data Acces object
            switch(DAType)
            {
                case 1:                 //.Net: processed in .Net Core lists
                    dA = new DA_Net();
                    break;
                
                case 2:                 //SQL Server: data stored and processed in SQL Server
                    //TO DO: implement DA_SQLServer class functionality!!
                    //dA = new DA_SQLServer();
                    //break;
                    return LFStatus.InvalidParameters;
                
                default:                //Error
                    return LFStatus.InvalidParameters;
            }


            try
            {
                string fileExtension = file.FileName.Substring(file.FileName.LastIndexOf('.')).ToLower();
                if (file.Length > 0 && (fileExtension == ".dat" || fileExtension == ".txt"))
                {
                    sr = new StreamReader(file.OpenReadStream());
                    if (dA.ProcessFile(ReadLine()) == 0)
                        retVal = LFStatus.OK;
                    else
                        retVal = LFStatus.LoadError;
                }
                else
                    retVal = LFStatus.InvalidFileName;
            }
            catch (Exception ex)
            {
                retVal = LFStatus.LoadError;
            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }

            return retVal;
        }


        public IEnumerable<DriversReport> GetReport()
        {
            return dA.GenerateReport();
        }


        /// <summary>
        /// Reads file and validates line length limit of maxLineLen
        /// </summary>
        /// <returns>Collection of strings with file content</returns>
        private IEnumerable<string> ReadLine()
        {
            int i;
            StringBuilder currentLine = new StringBuilder(maxLineLen);

            //We don't use ReadLine because it could be considered unsafe as it doesn't allow max line size
            while ((i = sr.Read()) != -1)
            {
                if (i == 10 || i == 13)     //LF: 10, CR: 13
                {
                    //Discard empty lines. This also handles CR-LF in any order
                    if (currentLine.Length > 0)
                    {
                        yield return currentLine.ToString();    //Return line
                        currentLine.Length = 0;                 //Reset stringbuilder
                    }
                    continue;
                }
                currentLine.Append((char)i);

                if (currentLine.Length > maxLineLen)
                    throw new InvalidOperationException("Line maximum length exceeded");
            }

            //Return last line
            if (currentLine.Length > 0)
                yield return currentLine.ToString();
        }
    }
}