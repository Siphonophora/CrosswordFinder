using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Formatting.Json;


namespace CrosswordPuzzle
{
    class Program
    {
        static void Main(string[] args)
        {
            var Size = int.Parse(args[0]);
            bool ProcessOnceSlice = args.Length == 2;
            int Slice = -1;
            if (ProcessOnceSlice)
            {
                int.TryParse(args[1] ?? null, out Slice);
            }
            string folder = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\CrosswordSearch\\" +
                $"{Size}\\{(ProcessOnceSlice ? Slice.ToString("00") : "All")}";

            //**************  Logger Setup ********************************
            string logFolder = $"{folder}";
            string sessionGUID = Guid.NewGuid().ToString();

            Serilog.Debugging.SelfLog.Enable(Console.Error);
            Log.Logger = new LoggerConfiguration()
            //.WriteTo.Console()
            .WriteTo.File($"{logFolder}\\log.txt", rollingInterval: RollingInterval.Day)
            .WriteTo.File(new JsonFormatter(), $"{logFolder}\\log.json", rollingInterval: RollingInterval.Day)
            .CreateLogger();


            //**************  MainProgram ********************************
            try
            {
                Log.Logger.Information("Startup Size: {Size} Slice {Slice}", Size, Slice);
                Analysis.Run(Size, Slice);
            }
            catch (Exception e)
            {
                Log.Logger.Fatal(e, "An unhandled excpetion terminated the program");
            }
            finally
            {
                Log.Logger.Information("Application Exiting");
                Log.CloseAndFlush();
            }
        }
    }
}
