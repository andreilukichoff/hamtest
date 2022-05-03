using System;

namespace HamTestWasmHosted.Server.Domain
{
    public class Token
    {
        public DateTime ExpiresAt { get; set; } 
        public int Seed { get; set; }
    }
}