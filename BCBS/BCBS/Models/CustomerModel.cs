using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BCBS.Models
{
    public class CustomerModel
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Customer Code")]
        public string ChargeCode { get; set; }
        //[Remote("IsCodeAvailable", "Project", ErrorMessage = "Code Already Exists")]
        [Required]
        [Display(Name = "Customer Type")]
        public string CustomerType { get; set; }
        [Required]
        [Display(Name = "Street Address")]
        public string CustomerAddress { get; set; }
        public string City { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Occupation { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Status { get; set; }
    }

    public class InvoiceViewModel
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public long ContractId { get; set; }
        [Display(Name = "Invoice Number")]
        [Required]
        public string InvoiceNumber { get; set; }
        [Required]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }
        [Display(Name = "Prepared By")]
        [Required]
        public string PrepareBy { get; set; }
        [Display(Name = "Prepared By Ext")]
        [Required]
        public string PrepareByExt { get; set; }
        [Required]
        [Display(Name = "Authorized By")]
        public string AuthorizedBy { get; set; }
        [Required]
        [Display(Name = "Authorized By Ext")]
        public string AuthorizedByExt { get; set; }
        [Display(Name = "Division")]
        [Required]
        public string Division { get; set; }
        [Display(Name = "Deffered?")]
        public bool IsDeffered { get; set; }
        [Display(Name = "Deffered Account")]
        public string DefferedAccount { get; set; }
        [Display(Name = "From Date")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FromDate { get; set; }
        [Display(Name = "To Date")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ToDate { get; set; }
        [Display(Name = "Special Instruction")]
        public string SpecialInstuction { get; set; }
        [Display(Name = "Supporting Document")]
        public string SupportingDocuments { get; set; }
        public double TotalAmount { get; set; }
        public CustomerModel Customer { get; set; }
    }

    public class ContractActivityViewModel
    {
        public long Id { get; set; }
        public long ContractId { get; set; }
        [Display(Name = "Invoice Number")]
        [Required]
        public string InvoiceNumber { get; set; }
        [Required]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }
        [Display(Name = "Prepared By")]
        [Required]
        public string PrepareBy { get; set; }
        [Display(Name = "Prepared By Ext")]
        [Required]
        public string PrepareByExt { get; set; }
        [Required]
        [Display(Name = "Authorized By")]
        public string AuthorizedBy { get; set; }
        [Required]
        [Display(Name = "Authorized By Ext")]
        public string AuthorizedByExt { get; set; }
        [Display(Name = "Division")]
        [Required]
        public string Division { get; set; }
        [Display(Name = "Deffered?")]
        public bool IsDeffered { get; set; }
        [Display(Name = "Deffered Account")]
        public string DefferedAccount { get; set; }
        [Display(Name = "From Date")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FromDate { get; set; }
        [Display(Name = "To Date")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ToDate { get; set; }
        [Display(Name = "Special Instruction")]
        public string SpecialInstuction { get; set; }
        [Display(Name = "Supporting Document")]
        public string SupportingDocuments { get; set; }
        public double TotalAmount { get; set; }
        public CustomerModel Customer { get; set; }
    }

    public class InvoiceModel
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        [Display(Name = "Invoice Number")]
        [Required]
        public string InvoiceNumber { get; set; }
        [Required]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }
        [Display(Name = "Prepare By")]
        [Required]
        public string PrepareBy { get; set; }
        [Display(Name = "Prepare By Ext")]
        [Required]
        public string PrepareByExt { get; set; }
        [Required]
        [Display(Name = "Authorized By")]
        public string AuthorizedBy { get; set; }
        [Required]
        [Display(Name = "Authorized By Ext")]
        public string AuthorizedByExt { get; set; }
        [Display(Name = "Division")]
        [Required]
        public string Division { get; set; }
        [Display(Name = "Deffered?")]
        public bool IsDeffered { get; set; }
        [Display(Name = "Deffered Account")]
        public string DefferedAccount { get; set; }
        [Display(Name = "From Date")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FromDate { get; set; }
        [Display(Name = "To Date")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ToDate { get; set; }
        [Display(Name = "Special Instuction")]
        public string SpecialInstuction { get; set; }
        [Display(Name = "Supporting Documents")]
        public string SupportingDocumanets { get; set; }
    }
    public class ContractInvoiceModel
    {
        public string ProjectName { get; set; }
        public string ServiceName { get; set; }
        public string RC { get; set; }
        public string ProjectCode { get; set; }
        public double Amount { get; set; }
        public string Charges { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
        public long ActivityId { get; set; }
    }
    public class CustomerContractModel
    {
        public long ContractId { get; set; }
        public string ContractCode { get; set; }
        public string ProjectName { get; set; }
        public string ServiceName { get; set; }
        public double Amount { get; set; }
        public string Charges { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Estimate { get; set; }

    }

    public class SBFEmailViewModel
    {
        public InvoiceViewModel Invoice { get; set; }
        public CustomerModel Customer { get; set; }
        public List<ActivityListModel> ActivityList { get; set; }
        public ContractModel Contract { get; set; }
    }

    public class AcknowledgementModel
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public string Status { get; set; }
        public List<AcknowledgementProjectServiceModel> Projects { get; set; }
    }

    public class ProjectServiceListModel
    {
        public long ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public long ServiceId { get; set; }
        public string ServiceName { get; set; }
        public double Budget { get; set; }
        public string Volume { get; set; }
        public string FeesType { get; set; }
    }
    public class AcknowledgementProjectServiceModel
    {
        public ProjectModel Project { get; set; }
        public List<AcknowledgementServiceModel> Services { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class AcknowledgementListModel
    {
        public long Id { get; set; }
        public string CustomerName { get; set; }
        public string ProjectName { get; set; }
        public string Status { get; set; }
    }

    public class CustomerAcknowledgementServicesModel
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public long AcknowledementId { get; set; }
        public long ProjectId { get; set; }
        public long ServiceId { get; set; }
        public double Total { get; set; }
        public string Volume { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string FeesType { get; set; }
    }
}