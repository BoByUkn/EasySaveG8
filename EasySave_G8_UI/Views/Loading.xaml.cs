﻿using EasySave_G8_UI.View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasySave_G8_UI.Views
{
    /// <summary>
    /// Logique d'interaction pour Loading.xaml
    /// </summary>
    public partial class Loading : Page
    {
        private MainWindow currentMainWindow;
        private Loading currentLoading;
        private View_Model ViewMODEL;
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        public Loading()
        {
            InitializeComponent();
            currentMainWindow = Application.Current.MainWindow as MainWindow;
            currentLoading = currentMainWindow.Main.Content as Loading;
            ViewMODEL = new View_Model();
        }

        public void ProgressBar_Manage()
        {
            ProgressBar_New();

            while (true) 
            {
                Thread.Sleep(500);
                ProgressBar_Update();
            }
        }

        public void ProgressBar_New()
        {
            Thread currentThread = Thread.CurrentThread;
            ProgressBar progressBar = null;
            MainStackPanel.Dispatcher.Invoke(() =>
            {
                progressBar = new ProgressBar();
                progressBar.Minimum = 0;
                progressBar.Maximum = 100;
                progressBar.Margin = new Thickness(20, 20, 20, 20);
                progressBar.Width = 400;
                progressBar.Height= 30;
                progressBar.Name = currentThread.Name;
                MainStackPanel.Children.Add(progressBar);
            });
        }


        public void ProgressBar_Update() 
        {
            Thread currentThread = Thread.CurrentThread;
            ProgressBar progressBar = null;
            string PgName = currentThread.Name;

            currentMainWindow.Dispatcher.Invoke(() =>
            {
                foreach (var child in MainStackPanel.Children)
                {
                    if ((child as FrameworkElement)?.Name == PgName) 
                    {
                        _semaphore.Wait();
                        try 
                        {
                            progressBar = child as ProgressBar;
                            progressBar.Value = ViewMODEL.MV_Update_ProgressionBar(PgName);
                        }
                        catch { Exception ex; }
                        _semaphore.Release();
                    }
                }
            });
        }
    }
}
