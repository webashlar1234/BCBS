using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace wcfService.Model
{
    [DataContract]
    public class ServiceType
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public long ProjectId { get; set; }
        [DataMember]
        public string FeesType { get; set; }
        [DataMember]
        public double Budget { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Notes { get; set; }
        //[DataMember]
        //public string Volume { get; set; }
    }

    public class ServiceFeesType
    {
        public long Id { get; set; }
        public long ServiceId { get; set; }
        public string FeesType { get; set; }
        public double Amount { get; set; }
    }
    public class ServiceTypeList
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public long ProjectId { get; set; }
        [DataMember]
        public string FeesType { get; set; }
        [DataMember]
        public double Budget { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Notes { get; set; }
        [DataMember]
        public string ProjectName { get; set; }

    }
}