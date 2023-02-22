using EasySave_G8_UI.View_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public Loading()
        {
            InitializeComponent();
            currentMainWindow = Application.Current.MainWindow as MainWindow;
            currentLoading = currentMainWindow.Main.Content as Loading;
        }

        public async void ProgressBar_Add()
        {
            Thread currentThread = Thread.CurrentThread;
            ProgressBar progressBar = null;
            Label middle_label = null;
            Label left_label = null;
            Label right_label = null;
            MainStackPanel.Dispatcher.Invoke(() =>
            {
                progressBar = new ProgressBar();
                progressBar.Minimum = 0;
                progressBar.Maximum = 100;
                progressBar.Margin = new Thickness(20, 20, 20, 20);
                progressBar.Width = 400;
                progressBar.Height = 30;
                progressBar.Foreground = Brushes.DarkCyan;
                progressBar.Name = currentThread.Name;
                MainStackPanel.Children.Add(progressBar);

                middle_label = new Label();
                middle_label.Height = 50;
                middle_label.HorizontalAlignment = HorizontalAlignment.Center;
                middle_label.VerticalAlignment = VerticalAlignment.Center;
                middle_label.FontSize = 20;
                middle_label.Margin = new Thickness(20, -57, 20, 0);
                middle_label.Name = currentThread.Name + "label1";
                MainStackPanel.Children.Add(middle_label);

                left_label = new Label();
                left_label.Height = 50;
                left_label.HorizontalAlignment = HorizontalAlignment.Left;
                left_label.VerticalAlignment = VerticalAlignment.Center;
                left_label.FontSize = 20;
                left_label.Margin = new Thickness(50, -58, 20, 0);
                left_label.Content = currentThread.Name;
                left_label.Name = currentThread.Name + "label3";
                left_label.Foreground = Brushes.White;
                MainStackPanel.Children.Add(left_label);

                right_label = new Label();
                right_label.Height = 50;
                right_label.HorizontalAlignment = HorizontalAlignment.Right;
                right_label.VerticalAlignment = VerticalAlignment.Center;
                right_label.FontSize = 20;
                right_label.Margin = new Thickness(20, -58, 40, 0);
                right_label.Content = "Running...";
                right_label.Foreground = Brushes.White;
                right_label.Name = currentThread.Name + "label2";
                MainStackPanel.Children.Add(right_label);
            });
        }

        public void UpdatePGBar(ProgressChangedEventArgs e)
        {
            ProgressBar progressBar = null;
            Label label1 = null;
            Label label2 = null;
            foreach (var child in MainStackPanel.Children)
            {
                if ((child as FrameworkElement)?.Name == e.UserState.ToString())
                {
                    progressBar = child as ProgressBar;
                    progressBar.Value = e.ProgressPercentage;
                }
                if ((child as FrameworkElement)?.Name == e.UserState.ToString() + "label1")
                {
                    label1 = child as Label;
                    label1.Content = e.ProgressPercentage + "%";
                }
                if ((child as FrameworkElement)?.Name == e.UserState.ToString() + "label2")
                {
                    if (progressBar.Value == 100)
                    {
                        label2 = child as Label;
                        label2.Content = "Done !";
                    }
                }
            }
            if (e.ProgressPercentage == 100)
            {
                MainStackPanel.Children.Remove(label1);
                MainStackPanel.Children.Remove(label2);
                MainStackPanel.Children.Remove(progressBar);
            }
        }
    }
}
