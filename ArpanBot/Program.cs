using System;
using System.Drawing;
using System.DrawingCore;
using System.DrawingCore.Drawing2D;
using System.DrawingCore.Imaging;
using System.DrawingCore.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using ImageFormat = System.DrawingCore.Imaging.ImageFormat;
using RectangleF = System.DrawingCore.RectangleF;

namespace ArpanBot
{
    class Program
    {

        private readonly string COMMAND_PREFIX = "!arpan";
        
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        
        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _client.Log += Log;
            _client.MessageReceived += CheckMessage;
            
            var token = Environment.GetEnvironmentVariable("DISCORD_API_KEY");
            _client.SetGameAsync($"{COMMAND_PREFIX} help");
            
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        public async Task AprilFools(SocketMessage m)
        {
            await m.AddReactionAsync(Emote.Parse(":regional_indicator_c:"), RequestOptions.Default);
            await m.AddReactionAsync(Emote.Parse(":hash:"), RequestOptions.Default);
        }

        public bool CSharpOrRust(string text)
        {
            bool isCsharp =  (text.Contains("c#", StringComparison.InvariantCultureIgnoreCase) ||
                    (text.Contains("c", StringComparison.InvariantCultureIgnoreCase) &&
                     text.Contains("#", StringComparison.InvariantCultureIgnoreCase)) ||
                    (text.Contains("c", StringComparison.InvariantCultureIgnoreCase) &&
                     text.Contains("sharp", StringComparison.InvariantCultureIgnoreCase)) ||
                    (text.Contains("see", StringComparison.InvariantCultureIgnoreCase) &&
                     text.Contains("#", StringComparison.InvariantCultureIgnoreCase)) ||
                    (text.Contains("see", StringComparison.InvariantCultureIgnoreCase) &&
                     text.Contains("sharp", StringComparison.InvariantCultureIgnoreCase)) ||
                    (text.Contains("sea", StringComparison.InvariantCultureIgnoreCase) &&
                     text.Contains("#", StringComparison.InvariantCultureIgnoreCase)) ||
                    (text.Contains("sea", StringComparison.InvariantCultureIgnoreCase) &&
                     text.Contains("sharp", StringComparison.InvariantCultureIgnoreCase)) ||
                    (text.Contains("cee", StringComparison.InvariantCultureIgnoreCase) &&
                     text.Contains("#", StringComparison.InvariantCultureIgnoreCase)) ||
                    (text.Contains("cee", StringComparison.InvariantCultureIgnoreCase) &&
                     text.Contains("sharp", StringComparison.InvariantCultureIgnoreCase)));

            bool isRust = (text.Contains("rust", StringComparison.InvariantCultureIgnoreCase)) ||
                          (text.Remove(' ').Contains("rust", StringComparison.InvariantCultureIgnoreCase));
            return isRust || isCsharp;
        }

        private async Task CheckMessage(SocketMessage m)
        {
            
            
            if (!(m is SocketUserMessage message))
                return;

            SocketUserMessage userMessage = (SocketUserMessage) m;

            if (m.Author.Id == 397223060446511114 || m.Author.Id == 373626821486575616 || new Random(DateTime.UtcNow.Millisecond).Next(0, 10) > 7 || CSharpOrRust(m.Content))
            {
                await AprilFools(m);
            }
            
            
            string[] args = userMessage.Content.Split(' ');
            if (args.Length >= 2 && args[0] == COMMAND_PREFIX && args[1] == "help")
            {
                await userMessage.ReplyAsync($"" +
                                             $"__ArpanBot Help__\n" +
                                             $"**!arpan help** - this screen\n" +
                                             $"**!arpan [text] ** - get arpan to say something");
             return;
            }
            
            
            try
            {
                string[] strs = userMessage.Content.Split(' ');

                if (userMessage.Content.Length >= COMMAND_PREFIX.Length && userMessage.Content.Substring(0, COMMAND_PREFIX.Length) == COMMAND_PREFIX)
                {

                    if (strs.Length >= 2)
                    {
                        string text = userMessage.Content.Substring(userMessage.Content.IndexOf(' '));
                        if (CSharpOrRust(text))
                        {
                            text = "C# is the best";
                        }
                        
                        Bitmap arpan = new Bitmap("arpan-3.png");
                        
                        
                        
                        
                        
                        RectangleF rectf = new RectangleF(1786, -600, 400, 500);

                        Graphics g = Graphics.FromImage(arpan);

                        g.RotateTransform(25);
                        g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit; 
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        g.DrawString(text, new Font("Tahoma",50, FontStyle.Bold), Brushes.Red, rectf);
                       
                        g.Flush();

                        
                        
                        
                        int randy = new Random().Next();
                        new Bitmap(arpan, arpan.Width/4, arpan.Height/4).Save($"{randy}-cimg.png", ImageFormat.Png);
                        
                        await userMessage.Channel.SendFileAsync($"./{randy}-cimg.png",
                            $"Image for {userMessage.Author.Mention}");
                        
                        File.Delete($"{randy}-cimg.png");
                    }
                }
                
            }
            catch (Exception e)
            {
                await userMessage.ReplyAsync("ArpanBot Encountered an error.");
                await _client.GetUser(397223060446511114).SendMessageAsync($"Error({e.Message}):\n{e.StackTrace}");
            }
        }
        
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
