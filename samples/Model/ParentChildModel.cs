using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nancy.Metadata.OpenApi.DemoApplication.Model
{
    public class ParentChildModel
    {
        public IEnumerable<SimpleResponseModel> Responses { get; set; }
    }
}
