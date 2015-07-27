using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace wcfService.Model
{
    [DataContract]
    class Project
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ChargeCode { get; set; }
        [DataMember]
        public string HighLevelBudget { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string RC { get; set; }
        [DataMember]
        public string GLAccount { get; set; }
    }
}
