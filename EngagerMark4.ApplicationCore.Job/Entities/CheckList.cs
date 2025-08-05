using EngagerMark4.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Job.Entities
{
    /// <summary>
    /// PCR2021
    /// ShortText1 = Salary Pay Item Code
    /// </summary>
    [Serializable]
    [Table("Tb_Checklist", Schema = "Job")]
    public class CheckList : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Code
        {
            get;
            set;
        }

        [Required]
        [StringLength(256)]
        public string Name
        {
            get;
            set;
        }

        public Int64 GLCodeId
        {
            get;
            set;
        }

        [NotMapped]
        public string SalaryPayItemCode
        {
            get
            {
                return this.ShortText1;
            }
            set
            {
                this.ShortText1 = value;
            }
        }

        public ControlType Type
        {
            get;
            set;
        } = ControlType.Checkbox;

        public enum ControlType
        {
            Checkbox,
            TextBox,
            Label
        }

        public override string ToString()
        {
            return nameof(CheckList) + " : " + Name;
        }
    }
}
