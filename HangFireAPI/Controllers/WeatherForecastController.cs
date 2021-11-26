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
            //Fire and Forget Jobs - S�o executados apenas uma �nica vez e quase que imediatamente ap�s a cria��o
            BackgroundJob.Enqueue(() => Helpers.WeatherForecastHelper.MeuPrimeiroJobFireAndForget());

            //Recurring Jobs - Faz o agendamento recorrente do processo a ser executado. S�o tarefas que executam de tempos em tempos.
            //Usa o m�todo RecurringJob.AddOrUpdate, o m�todo a ser executado e o CRON definido. (Podemos passar um fuso hor�rio e a fila onde desejamos processar)
            //A classe CRON cont�m diferentes m�todos e sobrecargas para executar jobs por minuto, hora, dia, semana, m�s e ano
            RecurringJob.AddOrUpdate(() => Helpers.WeatherForecastHelper.MeuPrimeiroRecurringJob(), Cron.Daily);

            //Delayed Jobs - Faz o agendamento do processo a ser executado. Usa o m�todo BackgroundJob.Schedule, o m�todo e o CRON que especifica
            //o agendamento da tarefa. A execu��o acontece no per�odo especificado (n�o executa imediatamente)
            BackgroundJob.Schedule(() => Helpers.WeatherForecastHelper.MeuPrimeiroScheduleJob(), TimeSpan.FromDays(7));

            //Continuations: S�o "tarefas filhas" cujas execu��es acontecem ap�s a tarefa pai ser processada.
            //Faz com que um processo seja executado novamente, pelo Id da execu��o.
            //Usa o m�todo BackgroundJob.ContinueJobWith, o id do Job pai e a ��o a ser processada (job filha).
            string jobId = BackgroundJob.Enqueue(() => Console.WriteLine("Tarefa pai"));
            BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine("Tarefa filha"));
        }
    }
}