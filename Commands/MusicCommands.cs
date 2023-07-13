using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Net;
using DSharpPlus.Lavalink;
using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System.Diagnostics;

namespace RadioBot.Commands {
    internal class MusicCommands : BaseCommandModule {
        DiscordClient client;

        public MusicCommands() {
            this.client = Program.Discord;
        }


        [Command("debug")]
        public async Task debugCommand(CommandContext ctx) {
            //await ctx.CreateResponseAsync("Hello, " + ctx.Member.Username + ".");
            await ctx.Client.SendMessageAsync(ctx.Channel, "test");
        }

        [Command("start")]
        public async Task joinCommand(CommandContext ctx) {
            var channel = client.GetChannelAsync((ulong)(Debugger.IsAttached ? 1061310811278159897 : 1119413956503683102));

            var lava = ctx.Client.GetLavalink();
            if (!lava.ConnectedNodes.Any()) {
                await ctx.RespondAsync("The Lavalink connection is not established/");
                return;
            }

            var node = lava.ConnectedNodes.Values.First();

            if (channel.Result.Type != ChannelType.Voice) {
                await ctx.RespondAsync("Not a valid voice channel.");
                return;
            }

            await node.ConnectAsync(channel.Result);
            await ctx.RespondAsync($"Joined **{channel.Result.Name}**");
        }

        [Command("play")]
        public async Task Play(CommandContext ctx) {
            //Important to check the voice state itself first, 
            //as it may throw a NullReferenceException if they don't have a voice state.
            if (ctx.Member.VoiceState == null || ctx.Member.VoiceState.Channel == null) {
                await ctx.RespondAsync("You are not in a voice channel.");
                return;
            }

            var lava = ctx.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

            if (conn == null) {
                await ctx.RespondAsync("Lavalink is not connected.");
                return;
            }

            var loadResult = node.Rest.GetTracksAsync(new Uri("http://radio.secretclub.cloud:8000/"));

            var track = loadResult.Result.Tracks.First();

            await conn.PlayAsync(track);
        }
    }
}
