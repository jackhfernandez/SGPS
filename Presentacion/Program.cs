namespace Presentacion
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            while (true)
            {
                using var login = new Login();
                if (login.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                using var principal = new Principal();
                Application.Run(principal);
            }
        }
    }
}