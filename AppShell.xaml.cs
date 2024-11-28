namespace Gradpath
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
            Routing.RegisterRoute(nameof(CounselorDashboardPage), typeof(CounselorDashboardPage));
            Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
            Routing.RegisterRoute(nameof(RoleSelectionPage), typeof(RoleSelectionPage));
            Routing.RegisterRoute(nameof(CounselorAddDetailsPage), typeof(CounselorAddDetailsPage));
            Routing.RegisterRoute(nameof(StudentAddDetailsPage), typeof(StudentAddDetailsPage));
            Routing.RegisterRoute(nameof(StudentDetailPage), typeof(StudentDetailPage)); // Add this line

            // Remove the menu button globally Shell.SetFlyoutBehavior(this,
            Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
        }

        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            if (args.Target.Location.OriginalString.Contains("LoginPage") || args.Target.Location.OriginalString.Contains("AddDetailsPage"))
            {
                // Hide the menu bar
                Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
            }
            else
            {
                // Ensure menu bar remains hidden for other pages
                Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
            }
        }
    }

}