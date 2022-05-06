namespace SmartOfficeServer
{
    public enum CoffeeMachineStatus
    {
        OK,
        NoWater,
        NoCoffee,
        NoMilk,
        Dirty,
        MakingCoffee
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

        public async Task MakeCoffee(HttpResponse response)
        {
            if (Status == CoffeeMachineStatus.OK)
            {
                Status = CoffeeMachineStatus.MakingCoffee;
                await Task.Delay(5000);
                //await Waiter.WaitSeconds(5, async sec => await response.WriteAsync($"Осталось {sec} секунд"));
                Status = CoffeeMachineStatus.OK;
            }
            else await response.WriteAsync("Кофемашина не исправна, чтобы готовить кофе");
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
                CoffeeMachineStatus.MakingCoffee => "Готовится кофе",
                _ => throw new NotImplementedException(),
            };
        }
    }
}
