using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Logistics.Models
{
    [Table("LoadsRoute")]
    public class LoadsRoute
    {
        [Key]
        [Column("route_id")]
        [Required]
        [StringLength(20)]
        public string RouteId { get; set; }

        [Column("route_startDateTime")]
        [Required]
        public DateTime RouteStartDateTime { get; set; }

        [Column("route_endDateTime")]
        public DateTime? RouteEndDateTime { get; set; }

        [Column("route_totalWeight", TypeName = "decimal(10,4)")]
        public decimal? RouteTotalWeight { get; set; }

        [Column("route_coordinatorName")]
        [Required]
        [StringLength(100)]
        public string RouteCoordinatorName { get; set; }

        [Column("route_coordinatorContactNo")]
        [Required]
        [StringLength(15)]
        public string RouteCoordinatorContactNo { get; set; }

        [Column("route_coordinatorId")]
        [Required]
        [StringLength(20)]
        public string RouteCoordinatorId { get; set; }

        [Column("route_status")]
        [Required]
        [StringLength(20)]
        public string RouteStatus { get; set; } = "Assigned";

        [Column("route_lastHubspot")]
        [StringLength(100)]
        public string? RouteLastHubspot { get; set; }

        [Column("route_lastHubspotDatetime")]
        public DateTime? RouteLastHubspotDatetime { get; set; }

        [Column("route_startHubspotPincode")]
        [Required]
        public int RouteStartHubspotPincode { get; set; }

        [Column("route_startHubspot")]
        [Required]
        [StringLength(100)]
        public string RouteStartHubspot { get; set; }

        [Column("route_endHubspotPincode")]
        [Required]
        public int RouteEndHubspotPincode { get; set; }

        [Column("route_endHubspot")]
        [Required]
        [StringLength(100)]
        public string RouteEndHubspot { get; set; }

        [Column("route_truckId")]
        [StringLength(20)]
        public string? RouteTruckId { get; set; }

        [Column("route_loadIds")]
        public string? RouteLoadIds { get; set; } // Storing Load IDs as comma-separated string or JSON

        [Column("route_pickupSpots")]
        public string? RoutePickupSpots { get; set; } // Storing Load IDs as comma-separated string or JSON

        [Column("route_dropSpots")]
        public string? RouteDropSpots { get; set; } // Storing Load IDs as comma-separated string or JSON

        // // Navigation property (FK to Truck)
        // [ForeignKey("RouteTruckId")]
        // public virtual Truck? Truck { get; set; }


    }
}
