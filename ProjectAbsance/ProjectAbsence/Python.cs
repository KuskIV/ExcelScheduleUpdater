using IronPython.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAbsence
{
    class PyClass
    {
        /// <summary>
        /// This method calls the Python script which is edits the excel sheet
        /// based on different person, and when they met.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="exePath"></param>
        /// <param name="py"></param>
        /// <returns></returns>
        public string Call(string path, string exePath, PythonVar py)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = exePath;
            start.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\"", path, Convert(py.date), Convert(py.value), Convert(py.person), py.amount_person, py.amount_day);
            start.UseShellExecute = false;
            start.CreateNoWindow = true;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string stderr = process.StandardError.ReadToEnd();
                    string result = reader.ReadToEnd();
                    return stderr;
                }
            }
        }

        #region private methods

        string Convert(List<string> input)
        {
            string result = "";

            input.ForEach(x => result += x + ",");

            result = result.TrimEnd(result[result.Length - 1]);
            return result;
        }

        #endregion
    }
}
