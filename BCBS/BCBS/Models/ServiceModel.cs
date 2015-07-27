using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCBS.Models
{
    public class ServiceModel
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Fees Type")]
        public string FeesType { get; set; }

        [Required]
        [Display(Name = "Budget")]
        public double Budget { get; set; }

        [Display(Name = "Transaction Volume")]
        public string Volume { get; set; }

        [Required]
        public string Status { get; set; }

        [Display(Name = "Project")]
        public long? ProjectId { get; set; }

        public string Notes { get; set; }
    }

    public class ServiceFeesTypeModel
    {
        public long Id { get; set; }
        public long ServiceId { get; set; }
        public string FeesType { get; set; }
        public double Amount { get; set; }
    }

    public class ServiceTypeListModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long ProjectId { get; set; }
        public string FeesType { get; set; }
        public string Budget { get; set; }
        //public string Amounts { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public string ProjectName { get; set; }

    }

    public class AcknowledgementServiceModel
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Fees Type")]
        public string FeesType { get; set; }

        [Required]
        [Display(Name = "Budget")]
        public double Budget { get; set; }

        [Display(Name = "Transaction Volume")]
        public string Volume { get; set; }

        public string NewVolume { get; set; }
        [Required]
        public string Status { get; set; }

        public string Notes { get; set; }

        public double Total { get; set; }
    }

}
