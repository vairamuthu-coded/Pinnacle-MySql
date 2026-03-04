using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinnacle.Models
{
    public class HrPayModel
    {
        public long HrPayDetailsId { get; set; }
        public long HrPayDetailsId1 { get; set; }

        public string DocId { get; set; }
        public string DocDate { get; set; }
        public string FinYear { get; set; }

        public long CompCode { get; set; }
        public long CompName { get; set; }

        public string MidCard { get; set; }
        public string IdCardNo { get; set; }
        public string EmpName { get; set; }
        public DateTime Doj { get; set; }
        public string Dol { get; set; }
        public string Date { get; set; }

        public string UanNo { get; set; }
        public string EsiNo { get; set; }
        public string FatherName { get; set; }

        public string United { get; set; }
        public string Category { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }

        public string OrjPayableDays { get; set; }
        public string NhDays { get; set; }
        public string PayableDays { get; set; }
        public string GovtDaySalary { get; set; }
        public string OtWages { get; set; }

        public string BasicDa { get; set; }
        public string Basic { get; set; }
        public string Da { get; set; }
        public string Hra { get; set; }
        public string Others { get; set; }

        public string EBasic { get; set; }
        public string EDa { get; set; }
        public string EBasicDa { get; set; }
        public string EHra { get; set; }
        public string EOthers { get; set; }

        public string PayableOtHours { get; set; }
        public string OtAmount { get; set; }
        public string Incentive { get; set; }
        public string GovtGross { get; set; }

        public string PfAmount { get; set; }
        public string EsiAmount { get; set; }
        public string MessAmount { get; set; }
        public string OthersExp { get; set; }

        public string Advance { get; set; }
        public string CreditDate { get; set; }
        public string Deduction { get; set; }
        public string NetAmount { get; set; }

        public string BankAccountNo { get; set; }
        public string BankName { get; set; }
        public string IfscCode { get; set; }

        public string PayPeriod { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public string Active { get; set; }

        public long CompCode1 { get; set; }
        public long UserName { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string IpAddress { get; set; }
    }

}
