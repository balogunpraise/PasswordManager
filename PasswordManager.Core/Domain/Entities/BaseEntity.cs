﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Core.Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAd { get; set; }
        public DateTime UpdatedAt { get; set; }

        public BaseEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}
