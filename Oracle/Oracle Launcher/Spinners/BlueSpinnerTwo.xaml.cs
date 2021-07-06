﻿using System.Windows.Controls;

namespace Oracle_Launcher.Spinners
{
    /// <summary>
    /// Interaction logic for BlueSpinnerTwo.xaml
    /// </summary>
    public partial class BlueSpinnerTwo : UserControl
    {
        public BlueSpinnerTwo()
        {
            InitializeComponent();
        }

        public void Stop()
        {
            Spinner.Children.Clear();
        }
    }
}
