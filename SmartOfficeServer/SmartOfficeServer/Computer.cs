namespace SmartOfficeServer
{
    public class Computer
    {
        private static int _version = 0;

        public string Name { get; set; }
        public bool IsWorking { get; set; }

        public Computer()
        {
            Name = $"Компьютер #{++_version}";
            IsWorking = false;
        }
    }
}
