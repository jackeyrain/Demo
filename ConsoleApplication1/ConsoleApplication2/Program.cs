using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            Aries.Inbound aries = new Aries.Inbound();
            var r = aries.PickingList(new[] {
                 new Aries.PickingListInfo()
                 {
                      BomCode = "9348819-07",
                       ProductLine = "BMW20",
                        Site = "1152",
                         Qty=30m,
                 }
             });
        }
    }
}
