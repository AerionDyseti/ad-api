using AerionDyseti.API.Shared.Models;

namespace AerionDyseti.API.Housecup.Models
{
    public class HouseAssignment
    {
        public AerionDysetiUser User { get; set; }
        public House AssignedHouse { get; set; }
    }
}