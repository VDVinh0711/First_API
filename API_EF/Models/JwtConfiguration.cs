﻿namespace API_EF.Models
{
    public class JwtConfiguration
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
