using EduSpecDataService.Code;
using EduSpecDataService.Models;
using NLog;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace EduSpecDataService
{
    public partial class EduSpecDataService : ServiceBase
    {
        double ProcessIntervals;
        public System.Timers.Timer timer;
        bool CanContinue = true;
        public EduSpecDataService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Logging.LogEntry(LogLevel.Info, "Application Start.");
            SetUp();
            timer = new System.Timers.Timer() { Interval = ProcessIntervals };
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            Logging.LogEntry(LogLevel.Info, "Start timer");
            timer.Start();
        }

        public async void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            try
            {
                Logging.LogEntry(LogLevel.Info, "Stop Timer.....");
                timer.Stop();
                if (CanContinue == true)
                {
                    CanContinue = false;
                    DateTime DateTimeNow = DateTime.Now;

                    SysParams sys = await SyncData.GetSysParamsAsync(Convert.ToInt32(ConfigurationManager.AppSettings["InsID"]), "SysParams");
                    //Post data to WebAPIa----------------------------------------------------------------------------------------
                    Logging.LogEntry(LogLevel.Info, "Post Parent_Child data...");
                    await SyncData.SyncParent_ChildAsync(sys);
                    Logging.LogEntry(LogLevel.Info, "Post Parent_Child Successful");

                    Logging.LogEntry(LogLevel.Info, "Post Learners...");
                    await SyncData.SyncLearnersAsync(sys);
                    Logging.LogEntry(LogLevel.Info, "Post Learners Successful");

                    Logging.LogEntry(LogLevel.Info, "Post Parents...");
                    await SyncData.SyncParentsAsync(sys);
                    Logging.LogEntry(LogLevel.Info, "Post Parents Successful");

                    if(sys.IsSASAMSFinance == true)
                    {
                        Logging.LogEntry(LogLevel.Info, "Post Transactions...");
                        await SyncData.SyncTransactionsAsync(sys);
                        Logging.LogEntry(LogLevel.Info, "Post Transactions Successful");
                    }
                    
                }
                
                CanContinue = true;

                Logging.LogEntry(LogLevel.Info, "Start timer.....");
                timer.Start();
                
            }
            catch (Exception error)
            {
                Logging.LogEntry(LogLevel.Info, error.Message);
                CanContinue = true;
                timer.Start();
            }
        }

        protected override void OnStop()
        {
            Logging.LogEntry(LogLevel.Info, "Application Stop.");
        }

        private void SetUp()
        {
            try
            {
                Logging.LogEntry(LogLevel.Info, "Settings");
                ProcessIntervals = double.Parse(ConfigurationManager.AppSettings["ProcessInterval"]);
                Logging.LogEntry(LogLevel.Info, string.Format("Setting Process Interval [{0}]", ProcessIntervals));
            }
            catch (Exception xe)
            {
                Logging.LogEntry(LogLevel.Info, "Error:" + xe.Message);
            }
        }

    }
}
