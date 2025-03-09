using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;


namespace SurveyApi.Models
{
    public class UserAccount
    {
        [SwaggerIgnore]
        public string Full_Name { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar")]
        [StringLength(250)]
        public string User_Name { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar")]
        [StringLength(250)]
        public string Password { get; set; } = string.Empty;

    }
}
