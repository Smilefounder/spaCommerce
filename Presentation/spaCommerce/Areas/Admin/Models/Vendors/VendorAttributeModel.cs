﻿using System.Collections.Generic;
using FluentValidation.Attributes;
using spaCommerce.Areas.Admin.Validators.Vendors;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace spaCommerce.Areas.Admin.Models.Vendors
{
    /// <summary>
    /// Represents a vendor attribute model
    /// </summary>
    [Validator(typeof(VendorAttributeValidator))]
    public partial class VendorAttributeModel : BaseNopEntityModel, ILocalizedModel<VendorAttributeLocalizedModel>
    {
        #region Ctor

        public VendorAttributeModel()
        {
            Locales = new List<VendorAttributeLocalizedModel>();
            VendorAttributeValueSearchModel = new VendorAttributeValueSearchModel();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Vendors.VendorAttributes.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Vendors.VendorAttributes.Fields.IsRequired")]
        public bool IsRequired { get; set; }

        [NopResourceDisplayName("Admin.Vendors.VendorAttributes.Fields.AttributeControlType")]
        public int AttributeControlTypeId { get; set; }

        [NopResourceDisplayName("Admin.Vendors.VendorAttributes.Fields.AttributeControlType")]
        public string AttributeControlTypeName { get; set; }

        [NopResourceDisplayName("Admin.Vendors.VendorAttributes.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<VendorAttributeLocalizedModel> Locales { get; set; }

        public VendorAttributeValueSearchModel VendorAttributeValueSearchModel { get; set; }

        #endregion
    }

    public partial class VendorAttributeLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.Vendors.VendorAttributes.Fields.Name")]
        public string Name { get; set; }
    }
}