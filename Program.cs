using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using System.Collections;
using System;
using System.IO;
using System.Data;
using Microsoft.Data.Sqlite;


namespace TelegramBotExperiments
{

    class Program
    {
        static string kod = System.IO.File.ReadAllText(@"tokenTG.txt");
        static ITelegramBotClient bot = new TelegramBotClient(kod.ToString());
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                if (message.Text.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать, юный киноман!");
                    return;
                }
                if (message.Text.ToLower() == "/info")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Моя задача помочь пользователю в подборке фильма.");
                    return;
                }
                if (message.Text.ToLower() == "/help")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Для работы с ботом необходимо воспользоваться кнопками: выбрать категорию жанр, затем сам фильм из предложенных.");
                    return;
                }
                await botClient.SendTextMessageAsync(message.Chat, "Извините, я не могу Вас понять.");
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            //подключение событий
            {
                AllowedUpdates = { }, //получать все типы обновлений
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();

            //подключение БД
            using (var connection = new SqliteConnection("Data Source=Films.db"))
            {
                connection.Open();
            }
            Console.Read();
        }
    }
}
