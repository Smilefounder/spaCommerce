﻿using FluentValidation;
using spaCommerce.Areas.Admin.Models.Directory;
using Nop.Core.Domain.Directory;
using Nop.Data;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace spaCommerce.Areas.Admin.Validators.Directory
{
    public partial class MeasureWeightValidator : BaseNopValidator<MeasureWeightModel>
    {
        public MeasureWeightValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Shipping.Measures.Weights.Fields.Name.Required"));
            RuleFor(x => x.SystemKeyword).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Shipping.Measures.Weights.Fields.SystemKeyword.Required"));

            SetDatabaseValidationRules<MeasureWeight>(dbContext);
        }
    }
}