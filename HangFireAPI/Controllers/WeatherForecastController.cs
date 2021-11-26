using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace HangFireAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public void Get()
        {
            //Fire and Forget Jobs - São executados apenas uma única vez e quase que imediatamente após a criação
            BackgroundJob.Enqueue(() => Helpers.WeatherForecastHelper.MeuPrimeiroJobFireAndForget());

            //Recurring Jobs - Faz o agendamento recorrente do processo a ser executado. São tarefas que executam de tempos em tempos.
            //Usa o método RecurringJob.AddOrUpdate, o método a ser executado e o CRON definido. (Podemos passar um fuso horário e a fila onde desejamos processar)
            //A classe CRON contém diferentes métodos e sobrecargas para executar jobs por minuto, hora, dia, semana, mês e ano
            RecurringJob.AddOrUpdate(() => Helpers.WeatherForecastHelper.MeuPrimeiroRecurringJob(), Cron.Daily);

            //Delayed Jobs - Faz o agendamento do processo a ser executado. Usa o método BackgroundJob.Schedule, o método e o CRON que especifica
            //o agendamento da tarefa. A execução acontece no período especificado (não executa imediatamente)
            BackgroundJob.Schedule(() => Helpers.WeatherForecastHelper.MeuPrimeiroScheduleJob(), TimeSpan.FromDays(7));

            //Continuations: São "tarefas filhas" cujas execuções acontecem após a tarefa pai ser processada.
            //Faz com que um processo seja executado novamente, pelo Id da execução.
            //Usa o método BackgroundJob.ContinueJobWith, o id do Job pai e a ção a ser processada (job filha).
            string jobId = BackgroundJob.Enqueue(() => Console.WriteLine("Tarefa pai"));
            BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine("Tarefa filha"));
        }
    }
}