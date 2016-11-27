using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoTech.Framework
{
    public class BaseGoTechEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public long ID { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

    }
}
