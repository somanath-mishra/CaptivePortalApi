using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaptivePortal.API.Models
{
    public class RegisterViewModel
    {
        public string HostName { get; set; }

        public string HostPassword { get; set; }

        public string UserName { get; set; }

        public string UserPassword { get; set; }

        public string Email { get; set; }

        public string ConfirmPassword { get; set; }
    }
}