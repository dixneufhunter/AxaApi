using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace SurveyApi.Models
{
    public class UserAccount
    {
        [SwaggerIgnore]
        public string Full_Name { get; set; } = string.Empty;

        public string User_Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

    }
}
