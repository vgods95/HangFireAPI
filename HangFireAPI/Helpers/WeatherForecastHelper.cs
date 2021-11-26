namespace HangFireAPI.Helpers
{
    public static class WeatherForecastHelper
    {
        public static async Task MeuPrimeiroJobFireAndForget()
        {
            await Task.Run(() =>
            {
                Console.WriteLine("Bem vindo ao Hangfire!");
                //throw new Exception("Opa, algo deu errado no seu job!");
            });
        }

        public static async Task MeuPrimeiroRecurringJob()
        {
            await Task.Run(() =>
            {
                Console.WriteLine("Esse é um recurring job!");
            });
        }

        public static async Task MeuPrimeiroScheduleJob()
        {
            await Task.Run(() =>
            {
                Console.WriteLine("Esse é um schedule job!");
            });
        }
    }
}
