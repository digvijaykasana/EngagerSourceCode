using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.ApplicationCore.Dummy.Entities.Users
{
    [Serializable]
    public class UserVehicle : BasicEntity
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

        public Int64 VehicleId
        {
            get;
            set;
        }

        [NotMapped]
        public string Model
        {
            get;
            set;
        }

        [NotMapped]
        public string VehicleNo
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
            return nameof(UserVehicle) + " : " + Id;
        }
    }
}
