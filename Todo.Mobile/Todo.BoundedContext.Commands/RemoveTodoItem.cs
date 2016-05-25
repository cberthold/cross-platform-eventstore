﻿using Infrastructure.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.BoundedContext.Commands
{
    public class RemoveTodoItem : ICommand
    {
        public Guid TenantId { get; set; }

        public int ExpectedVersion { get; set; }
        public Guid ItemId { get; set; }
    }
}
