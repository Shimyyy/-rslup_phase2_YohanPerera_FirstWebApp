﻿namespace FirstWebApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }

    
        public class LoginRequest
        {
            public string? Username { get; set; }
            public string? Password { get; set; }
        }
    }





