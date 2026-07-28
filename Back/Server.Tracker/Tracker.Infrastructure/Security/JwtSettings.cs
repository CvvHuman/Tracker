using System;
using System.Collections.Generic;
using System.Text;

namespace Tracker.Infrastructure.Security
{
    public class JwtSettings
    {
        public const string SectionName = "JwtSettings";
        public string Secret { get; init; } = null!;
        public string Issuer { get; init; } = null!;
        public string Audience { get; init; } = null!;
        public int ExpiryInMinutes { get; init; }
    }
}
