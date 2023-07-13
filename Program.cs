using System;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using DSharpPlus.SlashCommands;
using DSharpPlus.VoiceNext;

namespace RadioBot {
    internal class Program {
        static void Main(string[] args) {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public static DiscordClient Discord { get; private set; }

        public Task Status(DiscordClient c, HeartbeatEventArgs e) {
            var x = new DiscordActivity(name: "Radio", type: ActivityType.Streaming);
            Discord.UpdateStatusAsync(x);
            return Task.CompletedTask;
        }
        public async Task MainAsync() {

            Discord = new DiscordClient(new DiscordConfiguration() {
                Token = Utils.GetToken(),
                TokenType = TokenType.Bot,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug,
                LogTimestampFormat = "yyyy/MM/dd - hh:mm:ss tt",
                Intents = DiscordIntents.All
            });

            Discord.Heartbeated += Status;

            //var commandsConfig = new SlashCommandsConfiguration { };
            var commands = Discord.UseCommandsNext(new CommandsNextConfiguration() { StringPrefixes = new[] { "!" } });

            //commands.RegisterCommands<Commands.MiscCommands>();
            commands.RegisterCommands<Commands.MusicCommands>();

            var endpoint = new ConnectionEndpoint {
                Hostname = "127.0.0.1",
                Port = 2333
            };

            var lavalinkConfig = new LavalinkConfiguration {
                Password = "", // redacted
                RestEndpoint = endpoint,
                SocketEndpoint = endpoint
            };

            var lavalink = Discord.UseLavalink();

            await Discord.ConnectAsync();
            await lavalink.ConnectAsync(lavalinkConfig); // Make sure this is after Discord.ConnectAsync().

            await Task.Delay(-1);

        }
    }
}