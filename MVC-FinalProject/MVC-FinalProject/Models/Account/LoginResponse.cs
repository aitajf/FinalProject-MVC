﻿namespace MVC_FinalProject.Models.Account
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
    }
}
