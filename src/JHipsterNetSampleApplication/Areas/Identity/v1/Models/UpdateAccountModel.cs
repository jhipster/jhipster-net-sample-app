using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JHipsterNetSampleApplication.Areas.Identity.v1.Models {
    public class UpdateAccountModel {
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public string Email { get; internal set; }
        public string LangKey { get; internal set; }
        public string ImageUrl { get; internal set; }
    }
}
