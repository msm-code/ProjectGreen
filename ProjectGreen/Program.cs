namespace ProjectGreen
{
    class Program
    {
        static void Main(string[] args)
        {
            using (MainWindow wnd = new MainWindow())
            {
                wnd.Run(60);
            }
        }
    }
}
