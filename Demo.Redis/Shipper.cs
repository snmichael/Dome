using System;

namespace Demo.Redis
{
    public  class Shipper
    {
        public long Id { get; set; }
        public string CompanyName { get; set; }
        public DateTime DateCreated { get; set; }
        public ShipperType ShipperType { get; set; }
        public Guid UniqueRef { get; set; }
    }

    public enum ShipperType
    {
        Trains,
        Planes,
        All,
        Automobiles
    }
}