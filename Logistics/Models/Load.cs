using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Logistics.Models
{
    [Table("Load")]
    public class Load
    {
        [Key]
        [Column("load_id")]
        [MaxLength(20)]
        public string LoadId { get; set; } = null!;

        [Column("load_reg_datetime")]
        public DateTime LoadRegDateTime { get; set; }

        [Column("load_type")]
        [MaxLength(1)]
        public string LoadType { get; set; } = null!;

        [Column("load_weight", TypeName = "decimal(8,2)")]
        public decimal? LoadWeight { get; set; }

        [Column("load_status")]
        [MaxLength(20)]
        public string LoadStatus { get; set; } = "UnAssigned";

        [Column("load_paymentStatus")]
        [MaxLength(20)]
        public string LoadPaymentStatus { get; set; } = "Pending";

        [Column("load_paymentAmount", TypeName = "decimal(10,2)")]
        public decimal? LoadPaymentAmount { get; set; }

        [Column("load_paymentMode")]
        [MaxLength(20)]
        public string? LoadPaymentMode { get; set; }

        // Pickup details
        [Column("pickup_name")]
        [MaxLength(100)]
        public string PickupName { get; set; } = null!;

        [Column("pickup_emailId")]
        [MaxLength(255)]
        public string PickupEmailId { get; set; } = null!;

        [Column("pickup_contactNo")]
        [MaxLength(15)]
        public string PickupContactNo { get; set; } = null!;

        [Column("pickup_pincode")]
        public int PickupPincode { get; set; }

        [Column("pickup_country")]
        [MaxLength(100)]
        public string PickupCountry { get; set; } = null!;

        [Column("pickup_state")]
        [MaxLength(100)]
        public string PickupState { get; set; } = null!;

        [Column("pickup_district")]
        [MaxLength(100)]
        public string PickupDistrict { get; set; } = null!;

        [Column("pickup_city")]
        [MaxLength(100)]
        public string PickupCity { get; set; } = null!;

        [Column("pickup_address")]
        [MaxLength(250)]
        public string PickupAddress { get; set; } = null!;

        // Drop details
        [Column("drop_name")]
        [MaxLength(100)]
        public string DropName { get; set; } = null!;

        [Column("drop_emailId")]
        [MaxLength(255)]
        public string DropEmailId { get; set; } = null!;

        [Column("drop_contactNo")]
        [MaxLength(15)]
        public string DropContactNo { get; set; } = null!;

        [Column("drop_pincode")]
        public int DropPincode { get; set; }

        [Column("drop_country")]
        [MaxLength(100)]
        public string DropCountry { get; set; } = null!;

        [Column("drop_state")]
        [MaxLength(100)]
        public string DropState { get; set; } = null!;

        [Column("drop_district")]
        [MaxLength(100)]
        public string DropDistrict { get; set; } = null!;

        [Column("drop_city")]
        [MaxLength(100)]
        public string DropCity { get; set; } = null!;

        [Column("drop_address")]
        [MaxLength(250)]
        public string DropAddress { get; set; } = null!;

        // Optional foreign keys
        [Column("load_routeId")]
        [MaxLength(20)]
        public string? LoadRouteId { get; set; }

        [Column("load_pickupHubspot")]
        [MaxLength(20)]
        public string? LoadPickupHubspot { get; set; }

        [Column("load_dropHubspot")]
        [MaxLength(20)]
        public string? LoadDropHubspot { get; set; }

        [Column("load_expectedDeliveryDate")]
        public DateTime? LoadExpectedDeliveryDate { get; set; }

        [Column("load_actualDeliveryDate")]
        public DateTime? LoadActualDeliveryDate { get; set; }

        [Column("load_pickupHubspotPincode")]
        public int LoadPickupHubspotPincode { get; set; }

         [Column("load_dropHubspotPincode")]
        public int LoadDropHubspotPincode { get; set; }

        // Navigation properties (optional)
        // [ForeignKey("LoadPickupHubspot")]
        // public virtual Hubspot? PickupHubspot { get; set; }

        // [ForeignKey("LoadDropHubspot")]
        // public virtual Hubspot? DropHubspot { get; set; }

        // [ForeignKey("LoadRouteId")]
        // public virtual Route? Route { get; set; }
    }
}
