using System;
using System.Collections.Generic;
using System.Text;

namespace Elsy.UoCommon.Models
{
    public class UpdateItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public byte[] Binary { get; set; }

        public string FileName { get; set; }

        public long Components { get; set; }

        public DateTime CreateDate { get; set; }

        //public string CreateDate { get; set; }

        public string CreateDateByUser { get; set; }

        public bool IsDisabled { get; set; }

        /// <summary>
        /// код по
        /// </summary>
        public int SoId { get; set; }

        public string SoName { get; set; }

        public int? UfId { get; set; }  

        public long? DuId { get; set; }

  

    }
}
