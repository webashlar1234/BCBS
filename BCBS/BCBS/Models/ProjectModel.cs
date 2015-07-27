using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BCBS.Models
{
    public class ProjectModel
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name="Charge Code")]
        //[Remote("IsCodeAvailable", "Project", ErrorMessage = "Code Already Exists")]
        public string ChargeCode { get; set; }
        [Required]
        [Display(Name = "High Level Budget")]
        public string HighLevelBudget { get; set; }
        [Required]
        public string Status { get; set; }
        public string Description { get; set; }
        [Display(Name = "RC")]
        public string RC { get; set; }
        [Display(Name="G/L Account")]
        public string GLAccount { get; set; }
    }
}
