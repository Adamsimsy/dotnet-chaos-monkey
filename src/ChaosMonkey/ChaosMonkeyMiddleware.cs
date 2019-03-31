using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChaosMonkey
{
    public class ChaosMonkeyMiddleware
    {
        private readonly RequestDelegate _next;

        public ChaosMonkeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Do something with context near the beginning of request processing.

            if (!TerminateRequest())
            {
                await _next.Invoke(context);
            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(MonkeyAscii, Encoding.ASCII);
            }

            // Clean up.
        }

        private bool TerminateRequest()
        {
            var random = new Random();
            var randomLessThanTen = random.Next(10);

            return (randomLessThanTen > 5);
        }

        private static readonly string MonkeyAscii = "               Chaos Monkey! \r\n                __------__ \r\n              /~          ~\\ \r\n             |    //^\\//^\\| \r\n           /~~\\  ||  o| |o|:~\\ \r\n          | |6   ||___|_|_||:| \r\n           \\__.  /      o  \\/' \r\n            |   (       O   ) \r\n   /~~~~\\    `\\  \\         / \r\n  | |~~\\ |     )  ~------~`\\ \r\n /' |  | |   /     ____ /~~~)\\ \r\n(_/'   | | |     /'    |    ( | \r\n       | | |     \\    /   __)/ \\ \r\n       \\  \\ \\      \\/    /' \\   `\\ \r\n         \\  \\|\\        /   | |\\___| \r\n           \\ |  \\____/     | | \r\n           /^~>  \\        _/ < \r\n          |  |         \\       \\ \r\n          |  | \\        \\        \\ \r\n          -^-\\  \\       |        ) \r\n               `\\_______/^\\______/ \r\n";
    }

    public static class ChaosMonkeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseChaosMonkeyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ChaosMonkeyMiddleware>();
        }
    }
}
