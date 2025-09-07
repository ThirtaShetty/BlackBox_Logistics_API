using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Logistics.Models
{
    [Table("Truck")]
    public class Truck
    {
        [Key]
        [Column("truck_id")]
        [Required]
        [StringLength(20)]
        public string TruckId { get; set; }  

        [Column("truck_reg_dateTime")]
        [Required]
        public DateTime TruckRegDateTime { get; set; }

        [Column("truck_vehicleNumber")]
        [Required]
        [StringLength(100)]
        public string TruckVehicleNumber { get; set; }  

        [Column("truck_vehicleModel")]
        [Required]
        [StringLength(100)]
        public string TruckVehicleModel { get; set; }  

        [Column("truck_vehicleCapacity")]
        [Required]
        public decimal TruckVehicleCapacity { get; set; }

        [Column("driver_name")]
        [Required]
        [StringLength(100)]
        public string DriverName { get; set; }  

        [Column("driver_emailId")]
        [Required]
        [StringLength(100)]
        public string DriverEmailId { get; set; }  

        [Column("driver_contactNo")]
        [Required]
        [StringLength(15)]
        public string DriverContactNo { get; set; }  

        [Column("driver_status")]
        [Required]
        [StringLength(20)]
        public string DriverStatus { get; set; } = "InActive";

        [Column("homeLocation_pincode")]
        [Required]
        public int HomeLocationPincode { get; set; }

        [Column("homeLocation_country")]
        [Required]
        [StringLength(100)]
        public string HomeLocationCountry { get; set; }  

        [Column("homeLocation_state")]
        [Required]
        [StringLength(100)]
        public string HomeLocationState { get; set; }  

        [Column("homeLocation_district")]
        [Required]
        [StringLength(100)]
        public string HomeLocationDistrict { get; set; }  

        [Column("homeLocation_city")]
        [Required]
        [StringLength(100)]
        public string HomeLocationCity { get; set; }  

        [Column("homeLocation_address")]
        [Required]
        [StringLength(250)]
        public string HomeLocationAddress { get; set; }  

        [Column("currentLocation_pincode")]
        [Required]
        public int CurrentLocationPincode { get; set; }

        [Column("currentLocation_country")]
        [StringLength(100)]
        public string CurrentLocationCountry { get; set; }  

        [Column("currentLocation_state")]
        [StringLength(100)]
        public string CurrentLocationState { get; set; }  

        [Column("currentLocation_district")]
        [StringLength(100)]
        public string CurrentLocationDistrict { get; set; }  

        [Column("currentLocation_city")]
        [StringLength(100)]
        public string CurrentLocationCity { get; set; }  

        // [Column("currentLocation_hubspot")]
        // [ForeignKey("Hubspot")]
        // [StringLength(20)]
        // public string? CurrentLocationHubspot { get; set; }

        // // Navigation property (FK to Hubspot)
        // public virtual Hubspot? Hubspot { get; set; }
    }
}
