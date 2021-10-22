using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace bot
{
    public class Handlers
    {
        private static ILogger<Handlers> _logger;
        public static MessageBuilder _messageBuilder;

        public Handlers(ILogger<Handlers> logger, MessageBuilder messageBuilder)
        {
            _messageBuilder = messageBuilder;
            _logger = logger;
        }

        public static Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken ctoken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException => $"Error occured with Telegram Client: {exception.Message}",
                _ => exception.Message
            };

            _logger.LogCritical(errorMessage);

            return Task.CompletedTask;
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken ctoken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageReceived(client, update.Message),
                UpdateType.EditedMessage => BotOnMessageEdited(client, update.EditedMessage),
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(client, update.CallbackQuery),
                UpdateType.InlineQuery => BotOnInlineQueryReceived(client, update.InlineQuery),
                UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(client, update.ChosenInlineResult),
                _ => UnknownUpdateHandlerAsync(client, update)
            };

            await handler;
        }

        private static async Task BotOnMessageEdited(ITelegramBotClient client, Message editedMessage)
        {
            throw new NotImplementedException();
        }

        private static async Task UnknownUpdateHandlerAsync(ITelegramBotClient client, Update update)
        {
            throw new NotImplementedException();
        }

        private static async Task BotOnChosenInlineResultReceived(ITelegramBotClient client, ChosenInlineResult chosenInlineResult)
        {
            throw new NotImplementedException();
        }

        private static async Task BotOnInlineQueryReceived(ITelegramBotClient client, InlineQuery inlineQuery)
        {
            throw new NotImplementedException();
        }

        private static async Task BotOnCallbackQueryReceived(ITelegramBotClient client, CallbackQuery callbackQuery)
        {
            throw new NotImplementedException();
        }

        private static async Task BotOnMessageReceived(ITelegramBotClient client, Message message)
        {
            var chatId = message.Chat.Id;
            if (message.Text == "/start")
            {
                var list = new List<KeyboardButton>();
                {

                    new KeyboardButton() { Text = "Share", RequestLocation = true };
                    new KeyboardButton() { Text = "Cancel" };
                }
                var markup = new ReplyKeyboardMarkup(list, resizeKeyboard: true);
                var a = await client.SendTextMessageAsync
                    (
                    chatId,
                    "Share location?",
                    replyMarkup: markup
                    );
            }

            if (message.Type == MessageType.Location)
            {
                var latitude = message.Location.Latitude;
                var longitude = message.Location.Longitude;
                latitude = (float)Math.Round(latitude, 6);
                longitude = (float)Math.Round(longitude, 6);
                await client.SendTextMessageAsync(
                        chatId,
                        "Joyingiz Qabul Qilindi",
                        ParseMode.Markdown,
                        replyMarkup: new ReplyKeyboardRemove());

                Console.WriteLine("{0};{1}", latitude, longitude);
            }



            switch (message.Text)
            {
                case "/start":
                    break;
            }

        }

    }
}