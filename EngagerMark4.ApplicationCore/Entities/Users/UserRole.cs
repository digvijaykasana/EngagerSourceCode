using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Entities.Users
{
    [Serializable]
    [Table("Tb_UserRole", Schema = "Core")]
    public class UserRole : BasicEntity
    {
        public Int64 UserId
        {
            get;set;
        }

        [ForeignKey("UserId")]
        public User User
        {
            get;
            set;
        }

        public Int64 RoleId
        {
            get;
            set;
        }

        [ForeignKey("RoleId")]
        public Role Role
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
            return nameof(UserRole) + " : " + Id;
        }
    }
}
