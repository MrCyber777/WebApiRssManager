﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class RegistrationResponseDTO
    {
        public bool IsRegistrationSuccessfull { get; set; }
        public IEnumerable<string> RegistrationErrors { get; set; }
    }
}
