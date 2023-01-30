using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class AuthenticationResponseDTO
    {
        public bool IsAuthenticationSuccessfull { get; set; }
        public string ErrorMessage { get; set; }
        public string Token { get; set; }
        public UserDTO User { get; set; }
    }
}
