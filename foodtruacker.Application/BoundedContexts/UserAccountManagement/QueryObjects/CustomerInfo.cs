﻿using foodtruacker.QueryRepository.Repository;

namespace foodtruacker.Application.BoundedContexts.UserAccountManagement.QueryObjects
{
    public class CustomerInfo : QueryEntity
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
