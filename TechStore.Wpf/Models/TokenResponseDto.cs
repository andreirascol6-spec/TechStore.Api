using System;
using System.Collections.Generic;
using System.Text;

namespace TechStore.Wpf.Models
{
    public class TokenResponseDto
    {
        public required string AccesToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
