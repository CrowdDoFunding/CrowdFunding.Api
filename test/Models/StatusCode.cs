using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFundingTest.Models {
    public enum StatusCode
    {
        Ok = 200,
        Created = 201,
        Accepted = 202,
        NoContext = 204,
        BadRequest = 400,
        Unauthorized = 401,
        Forbitten = 403,
        NotFound = 404,
        InternalServerError = 500
    }
}
