namespace SmartOfficeServer
{
    public enum CoffeeMachineStatus
    {
        OK,
        NoWater,
        NoCoffee,
        NoMilk,
        Dirty
    }

    public class CoffeeMachine
    {
        private static int _version = 0;

        public string Name { get; set; }
        public CoffeeMachineStatus Status { get; set; }

        public CoffeeMachine()
        {
            Name = $"Кофемашина #{++_version}";
            Status = CoffeeMachineStatus.OK;
        }

        public string GetStatus()
        {
            return Status switch
            {
                CoffeeMachineStatus.OK => "Исправна",
                CoffeeMachineStatus.NoWater => "Мало воды",
                CoffeeMachineStatus.NoCoffee => "Мало кофейных зёрен",
                CoffeeMachineStatus.NoMilk => "Мало молока",
                CoffeeMachineStatus.Dirty => "Загрязнена",
                _ => throw new NotImplementedException(),
            };
        }
    }
}
