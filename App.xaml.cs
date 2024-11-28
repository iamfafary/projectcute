using Microsoft.Maui.Controls;
using System;

namespace Gradpath
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            string supabaseUrl = "https://boewazjeesdjnirgzbbu.supabase.co";
            string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImJvZXdhemplZXNkam5pcmd6YmJ1Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzE1NzU2MzMsImV4cCI6MjA0NzE1MTYzM30.pagd_l6tgBgdaObC1ZKHwi7Jw4MErksW--d6ySAdnYk";
            ServiceLocator.SupabaseClient = new SupabaseClient(supabaseUrl, supabaseKey);

            MainPage = new AppShell();
            Shell.Current.GoToAsync("//SplashPage");
        }
    }
}

