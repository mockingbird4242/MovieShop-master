using MovieShop.Core.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieShop.Core.ServiceInterfaces
{
  public  interface IJwtService
    {
        string GenerateJWT(LoginResponseModel userLoginResponse);
    }
}
