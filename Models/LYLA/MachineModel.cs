using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinnacle.Models.LYLA
{
   public class MachineModel
    {
        public MachineModel()
        {
        }
        public Int64 Asptblmacmasid { get; set; }
        public Int64 Asptblmacmas1id { get; set; }
        public Int64 Finyear { get; set; }
        public Int64 Compcode { get; set; }
        public string Linename { get; set; }
        public string Active { get; set; }

    }
   public class MachineModeldet: MachineModel
    {
      
        public Int64 Asptblmacdetid { get; set; }
        public string Machine { get; set; }
        public Int64 Processname { get; set; }       
        public string Notes { get; set; }



        public MachineModeldet()
        {
        }
        public MachineModeldet(long asptblmacmasid, string machine, long processname, string notes, long compcode, long asptblmacmas1id)
        {
            Asptblmacmasid = asptblmacmasid;
            Machine = machine;
            Processname = processname;
            Notes = notes;
            Compcode = compcode;
            Asptblmacmas1id = asptblmacmas1id;
            string ins = "insert into asptblmacdet(Asptblmacmasid,Machine,Processname,Notes,Compcode,Asptblmacmas1id)values('" + asptblmacmasid+"','"+machine+"','"+processname+"','"+notes+"','"+compcode+"','"+asptblmacmas1id+"')";
            Utility.ExecuteNonQuery(ins);
        }
        public MachineModeldet(long asptblmacmasid, string machine, long processname, string notes, long compcode, long asptblmacmas1id, long asptblmacdetid)
        {
            Asptblmacmasid = asptblmacmasid;
            Machine = machine;
            Processname = processname;
            Notes = notes;
            Compcode = compcode;
            Asptblmacmas1id = asptblmacmas1id;
            Asptblmacdetid = asptblmacdetid;
            string up = "update asptblmacdet set Asptblmacmasid='" + asptblmacmasid + "',Machine='" + machine + "',Processname='" + processname + "',Notes='" + notes + "',Compcode='" + compcode + "' where Asptblmacdetid='" + Asptblmacdetid + "' and Compcode='" + compcode + "'";
            Utility.ExecuteNonQuery(up);
        }
    }
}
