namespace MotorolaAssembler
{
    internal static class Program
    {
        /// <summary>
        /// This is a single-threaded application with Windows forms user interface.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}