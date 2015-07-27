using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCBS.Models
{
    public class ContractModel
    {
        public long Id { get; set; }
        [Required]
        [Display(Name = "Customer")]
        public long CustomerId { get; set; }

        [Required]
        [Display(Name = "Contract Code")]
        public string ContractCode { get; set; }

        [Required]
        [Display(Name = "Service Type")]
        public long ServiceId { get; set; }

        [Required]
        [Display(Name = "Project")]
        public long ProjectId { get; set; }

        [Display(Name = "From Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Charges")]
        public bool Dirrection { get; set; }

        public bool Estimate { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        [Display(Name="Fees Type")] 
        public string FeesType { get; set; }
        [Required]
        public double Amount { get; set; }
        public string Volume { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }

    }

    public class ContractListModel
    {
        public long Id { get; set; }
        public string Customer { get; set; }
        public string Service { get; set; }
        public string Project { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Dirrection { get; set; }
        public bool Estimate { get; set; }
        public string Status { get; set; }
        public double Amount { get; set; }
        public double? Volume { get; set; }
        public string Description { get; set; }
        public string ContractCode { get; set; }
    }

    public class ActivityModel
    {
        public long Id { get; set; }
        public long ContractId { get; set; }
        [Display(Name = "Contract Code")]
        public string ContractCode { get; set; }
        [Display(Name = "Activity Code")]
        public string ActivityCode { get; set; }
        public DateTime ActivityDate { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name="From Date")]
        public DateTime FromDate { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        public double Volume { get; set; }
        public double Amount { get; set; }
        public bool Charges { get; set; }
        public bool Estimate { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public bool IsBilled { get; set; }
        public string FileName { get; set; }

    }
    public class ActivityListModel
    {
        public long Id { get; set; }
        public long ContractId { get; set; }
        public DateTime ActivityDate { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FromDate { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        public double Volume { get; set; }
        public double Amount { get; set; }
        public bool Charges { get; set; }
        public bool Estimate { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string ContractCode { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string Service { get; set; }
        public bool IsBilled { get; set; }
        public string ActivityCode { get; set; }
        public string GLAccount { get; set; }
        public double Rate { get; set; }
        public double RateVolume { get; set; }
        public string FeesType { get; set; }
        public string RC { get; set; }
    }

}
