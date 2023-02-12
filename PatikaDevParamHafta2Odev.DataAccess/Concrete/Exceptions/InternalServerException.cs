using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PatikaDevParamHafta2Odev.DataAccess.Concrete.Exceptions
{
    public class InternalServerException : CustomException
    {
        public InternalServerException(string message) : base(message, null, HttpStatusCode.InternalServerError)
        {
        }
    }
}
