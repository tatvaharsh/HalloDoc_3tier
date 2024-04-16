using hallodoc_mvc_Repository.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hallocdoc_mvc_Service.Interface
{
    public interface IPDFService
    {
        byte[] GeneratePDF(Encounter en);
    }
}
