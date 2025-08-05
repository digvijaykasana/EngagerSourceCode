using EngagerMark4.ApplicationCore.Entities.Configurations;
using EngagerMark4.ApplicationCore.Dummy.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Entities.Users
{
    [Serializable]
    [Table("Tb_User", Schema = "Core")]
    public class User : BasicEntity
    {
        [StringLength(500)]
        public string FCMId
        {
            get;
            set;
        }

        [NotMapped]
        [StringLength(500)]
        public string FCMIdStr
        {
            get
            {
                return this.FCMId == null || FCMId.ToString() == String.Empty ? String.Empty : FCMId.ToString();
            }
        }

        [Required]
        public string ApplicationUserId
        {
            get;
            set;
        }

        [StringLength(256)]
        [Required]
        public string FirstName
        {
            get;
            set;
        }

        [StringLength(256)]
        [Required]
        public string LastName
        {
            get;
            set;
        }

        [NotMapped]
        public string Name
        {
            get
            {
                return LastName + " " + FirstName;
            }
        }

        [StringLength(256)]
        [Required]
        [Index(IsUnique = true)]
        public string UserName
        {
            get;
            set;
        }

        [StringLength(256)]
        [Required]
        public string Email
        {
            get;
            set;
        }

        [StringLength(256)]
        public string ContactNo
        {
            get;
            set;
        }

        public Int64 StatusId
        {
            get;
            set;
        }

        public string Address
        {
            get;
            set;
        }

        [StringLength(256)]
        public string Fax
        {
            get;
            set;
        }

        //PCR2021
        [StringLength(50)]
        public string NRIC
        {
            get;
            set;
        }

        [NotMapped]
        [StringLength(256)]
        public string PasswordStr
        {
            get;
            set;
        } = String.Empty;

        [NotMapped]
        public List<UserRole> UserRoleList
        {
            get; set;
        } = new List<UserRole>();

        public List<UserRole> GetRoleList()
        {
            if (UserRoleList == null)
                return new List<UserRole>();

            foreach (var role in UserRoleList.Where(x => x.Delete == false))
            {
                role.Id = 0;
                role.User = this;
                role.Role = null;
            }

            return UserRoleList.Where(x => x.Delete == false).ToList();
        }

        [NotMapped]
        public List<UserVehicle> VehicleList
        {
            get;
            set;
        } = new List<UserVehicle>();

        public List<UserVehicle> GetVehicleList()
        {
            if (VehicleList == null) return new List<UserVehicle>();

            foreach(var vehicle in VehicleList.Where(x => x.Delete == false))
            {
                vehicle.Id = 0;
                vehicle.User = this;
            }

            return VehicleList.Where(x => x.Delete == false).ToList();
        }

        [NotMapped]
        public List<UserCustomer> CustomerList
        {
            get;
            set;
        } = new List<UserCustomer>();

        public List<UserCustomer> GetCustomerList()
        {
            if (CustomerList == null) return new List<UserCustomer>();

            foreach(var customer in CustomerList.Where(x => x.Delete == false))
            {
                customer.Id = 0;
                customer.User = this;
            }

            return CustomerList.Where(x => x.Delete == false).ToList();
        }

        public override string ToString()
        {
            return nameof(User) + " : " + UserName;
        }
    }
}
