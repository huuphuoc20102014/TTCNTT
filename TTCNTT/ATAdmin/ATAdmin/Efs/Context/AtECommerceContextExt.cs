using System;
using ATAdmin.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ATAdmin.Efs.Entities
{
    public partial class AtECommerceContext : DbContext
    {
        internal string LoginUserId { get; set; }

        public AtECommerceContext() : base()
        {
        }
    }

    public class AtBaseECommerceEntity
    {
        public const string FULL_DB_CONTEXT_NAME = "AtECommerce.Efs.Entities.AtECommerceContext";
    }

    public class AtRegisterValidatorModel { }
    public class AtRegisterValidator : AtBaseValidator<AtRegisterValidatorModel> { }

}
