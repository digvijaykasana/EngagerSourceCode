using EngagerMark4.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.SOP.Entities.CreditNotes
{
    [Serializable]
    [Table("Tb_CreditNoteDetails", Schema = "SOP")]
    public class CreditNoteDetails : BaseEntity
    {
        public Int64 GLId
        {
            get;
            set;
        }

        public Int64 CreditNoteId
        {
            get;
            set;
        }

        [ForeignKey("CreditNoteId")]
        public CreditNote CreditNote
        {
            get;
            set;
        }

        public Int64 SerialNo
        {
            get;
            set;
        }

        [StringLength(500)]
        public string Description
        {
            get;
            set;
        }

        [Required]
        [Range(0, 999999999999999999.9999999999)]
        public decimal Qty
        {
            get;
            set;
        }

        [Required]
        [Range(0, 999999999999999999.9999999999)]
        public decimal Price
        {
            get;
            set;
        }

        [Required]
        [Range(0, 999999999999999999.9999999999)]
        public decimal TotalAmount
        {
            get;
            set;
        }

        [NotMapped]
        public bool Delete
        {
            get;
            set;
        }

        public override string ToString()
        {
            return nameof(CreditNoteDetails) + " : " + Id;
        }
    }
}
