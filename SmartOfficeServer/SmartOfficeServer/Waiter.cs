namespace SmartOfficeServer
{
    public static class Waiter
    {
        public static async Task WaitSeconds(int seconds, Func<int, Task> callback)
        {
            int elapsed = 0;
            while (elapsed < seconds)
            {
                await callback(seconds - elapsed);
                await Task.Delay(1000);
                elapsed++;
            }
        }
    }
}
