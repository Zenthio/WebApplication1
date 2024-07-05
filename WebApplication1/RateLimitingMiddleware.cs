using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace WebApplication1.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RateLimitingMiddleware> _logger;
        private static readonly ConcurrentDictionary<string, RateLimitInfo> RateLimits = new ConcurrentDictionary<string, RateLimitInfo>();

        private static readonly TimeSpan TimeWindow = TimeSpan.FromMinutes(1);
        private const int MaxRequests = 10;

        public RateLimitingMiddleware(RequestDelegate next, ILogger<RateLimitingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var remoteIp = context.Connection.RemoteIpAddress?.ToString();
            if (remoteIp == null)
            {
                await _next(context);
                return;
            }

            var rateLimitInfo = RateLimits.GetOrAdd(remoteIp, new RateLimitInfo());
            lock (rateLimitInfo)
            {
                if (rateLimitInfo.Timestamp + TimeWindow < DateTime.UtcNow)
                {
                    rateLimitInfo.Timestamp = DateTime.UtcNow;
                    rateLimitInfo.RequestCount = 0;
                }

                rateLimitInfo.RequestCount++;

                if (rateLimitInfo.RequestCount > MaxRequests)
                {
                    _logger.LogWarning($"Rate limit exceeded for IP: {remoteIp}");
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.Response.Headers["Retry-After"] = TimeWindow.TotalSeconds.ToString();
                    return;
                }
            }

            await _next(context);
        }

        private class RateLimitInfo
        {
            public DateTime Timestamp { get; set; } = DateTime.UtcNow;
            public int RequestCount { get; set; } = 0;
        }
    }
}
