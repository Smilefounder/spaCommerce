﻿using System.Collections.Generic;
using FluentValidation.Attributes;
using spaCommerce.Areas.Admin.Validators.Orders;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace spaCommerce.Areas.Admin.Models.Orders
{
    /// <summary>
    /// Represents a return request action model
    /// </summary>
    [Validator(typeof(ReturnRequestActionValidator))]
    public partial class ReturnRequestActionModel : BaseNopEntityModel, ILocalizedModel<ReturnRequestActionLocalizedModel>
    {
        #region Ctor

        public ReturnRequestActionModel()
        {
            Locales = new List<ReturnRequestActionLocalizedModel>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Configuration.Settings.Order.ReturnRequestActions.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.Order.ReturnRequestActions.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<ReturnRequestActionLocalizedModel> Locales { get; set; }

        #endregion
    }

    public partial class ReturnRequestActionLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.Order.ReturnRequestActions.Name")]
        public string Name { get; set; }
    }
}