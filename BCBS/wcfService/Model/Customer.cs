using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace wcfService.Model
{
    [DataContract]
    public class Customer
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ChargeCode { get; set; }
        [DataMember]
        public string CustomerType { get; set; }
        [DataMember]
        public string CustomerAddress { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string PostalCode { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string Occupation { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Status { get; set; }
    }

    public class ContractInvoice
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

    public class CustomerContract
    {
        public long ContractId { get; set; }
        public string ContractCode { get; set; }
        public string ProjectName { get; set; }
        public string ServiceName { get; set; }
        public double Amount { get; set; }
        public string Estimate { get; set; }
        public string Charges { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }

    }

    public class ProjectServiceList
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

    public class AcknowledgementList
    {
        public long Id {get;set;}
        public string CustomerName {get;set;}
        public string ProjectName {get;set;}
        public string Status {get;set;}
    }

    public class CustomerAcknowledementService
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public long AcknowledementId { get; set; }
        public long ProjectId { get; set; }
        public long ServiceId { get; set; }
        public double Total { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Volume { get; set; }
        public string Status { get; set; }
        public string FeesType { get; set; }
    }
}