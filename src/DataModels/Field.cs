﻿using MtdKey.Storage.Context;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MtdKey.Storage.DataModels
{ 
    internal class Field : IFilterBasic, IFilterChild
    {
        public Field()
        {
            Stacks = new HashSet<Stack>();
        }

        [Key]
        public long Id { get; set; }
        /// <summary>
        /// Bunch ID value
        /// </summary>
        public long ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int FieldType { get; set; }        
        public byte ArchiveFlag { get; set; }
        public byte DeletedFlag { get; set; }

        public virtual Bunch Bunch { get; set; }
        public virtual ICollection<Stack> Stacks { get; set; }
    }
}