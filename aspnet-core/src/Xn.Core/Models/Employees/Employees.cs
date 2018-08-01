using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Xn.Models.Employees
{
   public class Employees:Entity
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime? Birthday { get; set; }
        public string Position { get; set; }
        [ForeignKey("Company")]
        public string Code { get; set; }
        public string KeyUser { get; set; }
        public int IdCty { get; set; }
        public long CreateIdUser { get; set; }
    }
}
