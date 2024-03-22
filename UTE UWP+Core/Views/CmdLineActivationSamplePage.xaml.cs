﻿using System;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace UTE_UWP_.Views
{
    // TODO: Remove this sample page when/if it's not needed.
    // This page is an sample of how to launch a specific page in response to launching from the command line and passing arguments.
    // It is expected that you will delete this page once you have changed the handling of command line launch to meet
    // your needs and redirected to another of your pages.
    public sealed partial class CmdLineActivationSamplePage : Page
    {
        public CmdLineActivationSamplePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            PassedArguments.Text = e.Parameter?.ToString();
        }
    }
}
