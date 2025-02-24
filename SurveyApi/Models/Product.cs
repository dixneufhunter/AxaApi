using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace SurveyApi.Models
{
    public class Product
    {

        //[JsonIgnore]
        [SwaggerIgnore]
        public int ID { get; set; }

        public string NAME { get; set; }

        public string DESCRIPTION { get; set; }

        public decimal PRICE { get; set; }

        public DateTime CREATEDAT { get; set; }
        //public ICollection<Transaction> Transactions { get; set; }

    }


}
