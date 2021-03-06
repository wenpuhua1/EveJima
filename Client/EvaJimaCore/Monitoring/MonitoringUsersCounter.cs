﻿using System;
using System.ComponentModel;
using System.Timers;
using System.Windows.Forms;
using EvaJimaCore;
using EveJimaCore.Tools;
using log4net;
using Timer = System.Timers.Timer;

namespace EveJimaCore.Monitoring
{
    public partial class MonitoringUsersCounter : UserControl
    {
        private readonly ILog _logger = LogManager.GetLogger(string.Empty);

        const int ERROR_FILE_NOT_FOUND = 2;
        const int ERROR_ACCESS_DENIED = 5;
        const int ERROR_NO_APP_ASSOCIATED = 1155;

        private readonly Timer _workerTimer;

        private readonly string address = "";

        public MonitoringUsersCounter()
        {
            InitializeComponent();

            _logger.Info("[MonitoringUsersCounter] Started users count monitoring");

            _workerTimer = new Timer();
            _workerTimer.Elapsed += Event_Refresh;
            _workerTimer.Interval = 60000;
            _workerTimer.Enabled = false;

            if (!Common.IsAppicationModeRuntime()) return;

            address = Global.ApplicationSettings.Common.StatisticVisitorsCounterPage;

            _workerTimer.Enabled = true;
            browserUsersCounterMetric.ScriptErrorsSuppressed = true;
            browserUsersCounterMetric.Navigate(address);
        }

        private void Event_Refresh(object sender, ElapsedEventArgs e)
        {
            _workerTimer.Enabled = false;

            EventNavigateInternalBrowser();

            _workerTimer.Enabled = true;
        }

        private void EventNavigateInternalBrowser()
        {
            try
            {
                _logger.Debug("Refresh users counter metric");
                browserUsersCounterMetric.Refresh();
            }
            catch (Win32Exception e)
            {
                if (e.NativeErrorCode == ERROR_FILE_NOT_FOUND || e.NativeErrorCode == ERROR_ACCESS_DENIED || e.NativeErrorCode == ERROR_NO_APP_ASSOCIATED)
                {
                    _logger.Error("[UsersCounterMonitoring.EventNavigateInternalBrowser] Critical error on updated user counter in address " + address + " Exception is " + e.Message);
                }
            }
            catch (Exception exception)
            {
                _logger.Error("[UsersCounterMonitoring.EventNavigateInternalBrowser] Critical error on updated user counter in address " + address + " Exception is " + exception.Message);
            }
            catch
            {
                _logger.Error("[UsersCounterMonitoring.EventNavigateInternalBrowser] Critical unexpected error on updated user counter in address " + address + " ");
            }

        }
    }
}
