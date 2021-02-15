using Microsoft.AspNetCore.Identity;
using System;

namespace foodtruacker.Authentication.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string UnConfirmedEmail { get; set; }
    }
}
