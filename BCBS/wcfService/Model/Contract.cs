using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace wcfService.Model
{
    [DataContract]
    public class Contract
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long CustomerId { get; set; }
        [DataMember]
        public long ServiceId { get; set; }
        [DataMember]
        public long ProjectId { get; set; }
        [DataMember]
        public DateTime FromDate { get; set; }
        [DataMember]
        public DateTime EndDate { get; set; }
        [DataMember]
        public bool Dirrection { get; set; }
        [DataMember]
        public bool Estimate { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public double Budget { get; set; }
        [DataMember]
        public string Volume { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string ContractCode { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string FeesType { get; set; }
    }

    public class ContractList
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string Customer { get; set; }
        [DataMember]
        public string Service { get; set; }
        [DataMember]
        public string Project { get; set; }
        [DataMember]
        public DateTime FromDate { get; set; }

        [DataMember]
        public DateTime EndDate { get; set; }

        [DataMember]
        public bool Dirrection { get; set; }
        [DataMember]
        public bool Estimate { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public double? Volume { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string ContractCode { get; set; }
    }

    public class Activity
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long ContractId { get; set; }
        [DataMember]
        public string ActivityCode { get; set; }
        [DataMember]
        public DateTime ActivityDate { get; set; }
        [DataMember]
        public DateTime FromDate { get; set; }
        [DataMember]
        public DateTime EndDate { get; set; }
        [DataMember]
        public double Volume { get; set; }
        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public bool Charges { get; set; }
        [DataMember]
        public bool Estimate { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public bool IsBilled { get; set; }
        [DataMember]
        public string FileName { get; set; }
    }
    public class ActivityList
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long ContractId { get; set; }
        [DataMember]
        public string ActivityCode { get; set; }
        [DataMember]
        public string ContractCode { get; set; }
        [DataMember]
        public string ProjectName { get; set; }
        [DataMember]
        public string ProjectCode { get; set; }
        [DataMember]
        public string GLAccount { get; set; }
        [DataMember]
        public string Service { get; set; }
        [DataMember]
        public DateTime ActivityDate { get; set; }
        [DataMember]
        public DateTime FromDate { get; set; }
        [DataMember]
        public DateTime EndDate { get; set; }
        [DataMember]
        public double Volume { get; set; }
        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public bool Charges { get; set; }
        [DataMember]
        public bool Estimate { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public bool IsBilled { get; set; }
        [DataMember]
        public double Rate { get; set; }
        [DataMember]
        public double RateVolume { get; set; }
        [DataMember]
        public string FeesType { get; set; }
        [DataMember]
        public string RC { get; set; }
    }
}