using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Logistics.Models
{
    [Table("Hubspot")]
    public class Hubspot
    {
        [Key]
        [Column("hubspot_id")]
        [Required]
        [StringLength(20)]
        public string HubspotId { get; set; }

        [Column("hubspot_name")]
        [Required]
        [StringLength(100)]
        public string HubspotName { get; set; }

        [Column("hubspot_inchargeName")]
        [Required]
        [StringLength(100)]
        public string HubspotInchargeName { get; set; }

        [Column("hubspot_contactNo")]
        [Required]
        [StringLength(15)]
        public string HubspotContactNo { get; set; }

        [Column("hubspot_pincode")]
        [Required]
        public int HubspotPincode { get; set; }

        [Column("hubspot_country")]
        [Required]
        [StringLength(100)]
        public string HubspotCountry { get; set; }

        [Column("hubspot_state")]
        [Required]
        [StringLength(100)]
        public string HubspotState { get; set; }

        [Column("hubspot_district")]
        [Required]
        [StringLength(100)]
        public string HubspotDistrict { get; set; }

        [Column("hubspot_city")]
        [Required]
        [StringLength(100)]
        public string HubspotCity { get; set; }

        [Column("hubspot_address")]
        [Required]
        [StringLength(250)]
        public string HubspotAddress { get; set; }
    }
}
