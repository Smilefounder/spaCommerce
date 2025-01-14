﻿using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Tax;
using Nop.Core.Domain.Vendors;
using Nop.Services.Common;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Vendors;
using spaCommerce.Factories;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Security;
using Nop.Web.Framework.Security.Captcha;
using Nop.Web.Framework.Themes;
using spaCommerce.Models.Common;

namespace spaCommerce.Controllers
{
    public partial class CommonController : BasePublicController
    {
        #region Fields

        private readonly CaptchaSettings _captchaSettings;
        private readonly CommonSettings _commonSettings;
        private readonly ICommonModelFactory _commonModelFactory;
        private readonly ICurrencyService _currencyService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;
        private readonly IStoreContext _storeContext;
        private readonly IThemeContext _themeContext;
        private readonly IVendorService _vendorService;
        private readonly IWorkContext _workContext;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly LocalizationSettings _localizationSettings;
        private readonly StoreInformationSettings _storeInformationSettings;
        private readonly VendorSettings _vendorSettings;
        
        #endregion
        
        #region Ctor

        public CommonController(CaptchaSettings captchaSettings,
            CommonSettings commonSettings,
            ICommonModelFactory commonModelFactory,
            ICurrencyService currencyService,
            ICustomerActivityService customerActivityService,
            IGenericAttributeService genericAttributeService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            ILogger logger,
            IStoreContext storeContext,
            IThemeContext themeContext,
            IVendorService vendorService,
            IWorkContext workContext,
            IWorkflowMessageService workflowMessageService,
            LocalizationSettings localizationSettings,
            StoreInformationSettings storeInformationSettings,
            VendorSettings vendorSettings)
        {
            this._captchaSettings = captchaSettings;
            this._commonSettings = commonSettings;
            this._commonModelFactory = commonModelFactory;
            this._currencyService = currencyService;
            this._customerActivityService = customerActivityService;
            this._genericAttributeService = genericAttributeService;
            this._languageService = languageService;
            this._localizationService = localizationService;
            this._logger = logger;
            this._storeContext = storeContext;
            this._themeContext = themeContext;
            this._vendorService = vendorService;
            this._workContext = workContext;
            this._workflowMessageService = workflowMessageService;
            this._localizationSettings = localizationSettings;
            this._storeInformationSettings = storeInformationSettings;
            this._vendorSettings = vendorSettings;
        }

        #endregion

        #region Methods

        //page not found
        public virtual IActionResult PageNotFound()
        {
            if (_commonSettings.Log404Errors)
            {
                var statusCodeReExecuteFeature = HttpContext?.Features?.Get<IStatusCodeReExecuteFeature>();
                //TODO add locale resource
                _logger.Error($"Error 404. The requested page ({statusCodeReExecuteFeature?.OriginalPath}) was not found",
                    customer: _workContext.CurrentCustomer);
            }
            
            Response.StatusCode = 404;
            Response.ContentType = "text/html";

            return View();
        }


        //robots.txt file
        //available even when a store is closed
        [CheckAccessClosedStore(true)]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual IActionResult RobotsTextFile()
        {
            var robotsFileContent = _commonModelFactory.PrepareRobotsTextFile();
            return Content(robotsFileContent, MimeTypes.TextPlain);
        }


        //SEO sitemap page
        [HttpsRequirement(SslRequirement.No)]
        //available even when a store is closed
        [CheckAccessClosedStore(true)]
        public virtual IActionResult SitemapXml(int? id)
        {
            if (!_commonSettings.SitemapEnabled)
                return RedirectToRoute("HomePage");

            var siteMap = _commonModelFactory.PrepareSitemapXml(id);
            return Content(siteMap, "text/xml");
        }

        ////available even when a store is closed
        //[CheckAccessClosedStore(true)]
        ////available even when navigation is not allowed
        //[CheckAccessPublicStore(true)]
        //public virtual IActionResult SetLanguage(int langid, string returnUrl = "")
        //{
        //    var language = _languageService.GetLanguageById(langid);
        //    if (!language?.Published ?? false)
        //        language = _workContext.WorkingLanguage;

        //    //home page
        //    if (string.IsNullOrEmpty(returnUrl))
        //        returnUrl = Url.RouteUrl("HomePage");

        //    //prevent open redirection attack
        //    if (!Url.IsLocalUrl(returnUrl))
        //        returnUrl = Url.RouteUrl("HomePage");

        //    //language part in URL
        //    if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
        //    {
        //        //remove current language code if it's already localized URL
        //        if (returnUrl.IsLocalizedUrl(this.Request.PathBase, true, out Language _))
        //            returnUrl = returnUrl.RemoveLanguageSeoCodeFromUrl(this.Request.PathBase, true);

        //        //and add code of passed language
        //        returnUrl = returnUrl.AddLanguageSeoCodeToUrl(this.Request.PathBase, true, language);
        //    }

        //    _workContext.WorkingLanguage = language;

        //    return Redirect(returnUrl);
        //}

        ////available even when navigation is not allowed
        //[CheckAccessPublicStore(true)]
        //public virtual IActionResult SetCurrency(int customerCurrency, string returnUrl = "")
        //{
        //    var currency = _currencyService.GetCurrencyById(customerCurrency);
        //    if (currency != null)
        //        _workContext.WorkingCurrency = currency;

        //    //home page
        //    if (string.IsNullOrEmpty(returnUrl))
        //        returnUrl = Url.RouteUrl("HomePage");

        //    //prevent open redirection attack
        //    if (!Url.IsLocalUrl(returnUrl))
        //        returnUrl = Url.RouteUrl("HomePage");

        //    return Redirect(returnUrl);
        //}

        ////available even when navigation is not allowed
        //[CheckAccessPublicStore(true)]
        //public virtual IActionResult SetTaxType(int customerTaxType, string returnUrl = "")
        //{
        //    var taxDisplayType = (TaxDisplayType)Enum.ToObject(typeof(TaxDisplayType), customerTaxType);
        //    _workContext.TaxDisplayType = taxDisplayType;

        //    //home page
        //    if (string.IsNullOrEmpty(returnUrl))
        //        returnUrl = Url.RouteUrl("HomePage");

        //    //prevent open redirection attack
        //    if (!Url.IsLocalUrl(returnUrl))
        //        returnUrl = Url.RouteUrl("HomePage");

        //    return Redirect(returnUrl);
        //}

        ////contact us page
        //[HttpsRequirement(SslRequirement.Yes)]
        ////available even when a store is closed
        //[CheckAccessClosedStore(true)]
        //public virtual IActionResult ContactUs()
        //{
        //    var model = new ContactUsModel();
        //    model = _commonModelFactory.PrepareContactUsModel(model, false);
        //    return View(model);
        //}

        //[HttpPost, ActionName("ContactUs")]
        //[PublicAntiForgery]
        //[ValidateCaptcha]
        ////available even when a store is closed
        //[CheckAccessClosedStore(true)]
        //public virtual IActionResult ContactUsSend(ContactUsModel model, bool captchaValid)
        //{
        //    //validate CAPTCHA
        //    if (_captchaSettings.Enabled && _captchaSettings.ShowOnContactUsPage && !captchaValid)
        //    {
        //        ModelState.AddModelError("", _captchaSettings.GetWrongCaptchaMessage(_localizationService));
        //    }

        //    model = _commonModelFactory.PrepareContactUsModel(model, true);

        //    if (ModelState.IsValid)
        //    {
        //        var subject = _commonSettings.SubjectFieldOnContactUsForm ? model.Subject : null;
        //        var body = Nop.Core.Html.HtmlHelper.FormatText(model.Enquiry, false, true, false, false, false, false);

        //        _workflowMessageService.SendContactUsMessage(_workContext.WorkingLanguage.Id,
        //            model.Email.Trim(), model.FullName, subject, body);

        //        model.SuccessfullySent = true;
        //        model.Result = _localizationService.GetResource("ContactUs.YourEnquiryHasBeenSent");

        //        //activity log
        //        _customerActivityService.InsertActivity("PublicStore.ContactUs", 
        //            _localizationService.GetResource("ActivityLog.PublicStore.ContactUs"));

        //        return View(model);
        //    }

        //    return View(model);
        //}

        ////contact vendor page
        //[HttpsRequirement(SslRequirement.Yes)]
        //public virtual IActionResult ContactVendor(int vendorId)
        //{
        //    if (!_vendorSettings.AllowCustomersToContactVendors)
        //        return RedirectToRoute("HomePage");

        //    var vendor = _vendorService.GetVendorById(vendorId);
        //    if (vendor == null || !vendor.Active || vendor.Deleted)
        //        return RedirectToRoute("HomePage");

        //    var model = new ContactVendorModel();
        //    model = _commonModelFactory.PrepareContactVendorModel(model, vendor, false);
        //    return View(model);
        //}

        //[HttpPost, ActionName("ContactVendor")]
        //[PublicAntiForgery]
        //[ValidateCaptcha]
        //public virtual IActionResult ContactVendorSend(ContactVendorModel model, bool captchaValid)
        //{
        //    if (!_vendorSettings.AllowCustomersToContactVendors)
        //        return RedirectToRoute("HomePage");

        //    var vendor = _vendorService.GetVendorById(model.VendorId);
        //    if (vendor == null || !vendor.Active || vendor.Deleted)
        //        return RedirectToRoute("HomePage");

        //    //validate CAPTCHA
        //    if (_captchaSettings.Enabled && _captchaSettings.ShowOnContactUsPage && !captchaValid)
        //    {
        //        ModelState.AddModelError("", _captchaSettings.GetWrongCaptchaMessage(_localizationService));
        //    }

        //    model = _commonModelFactory.PrepareContactVendorModel(model, vendor, true);

        //    if (ModelState.IsValid)
        //    {
        //        var subject = _commonSettings.SubjectFieldOnContactUsForm ? model.Subject : null;
        //        var body = Nop.Core.Html.HtmlHelper.FormatText(model.Enquiry, false, true, false, false, false, false);

        //        _workflowMessageService.SendContactVendorMessage(vendor, _workContext.WorkingLanguage.Id,
        //            model.Email.Trim(), model.FullName, subject, body);

        //        model.SuccessfullySent = true;
        //        model.Result = _localizationService.GetResource("ContactVendor.YourEnquiryHasBeenSent");

        //        return View(model);
        //    }

        //    return View(model);
        //}

        ////sitemap page
        //[HttpsRequirement(SslRequirement.No)]
        //public virtual IActionResult Sitemap(SitemapPageModel pageModel)
        //{
        //    if (!_commonSettings.SitemapEnabled)
        //        return RedirectToRoute("HomePage");

        //    var model = _commonModelFactory.PrepareSitemapModel(pageModel);
        //    return View(model);
        //}

        //public virtual IActionResult SetStoreTheme(string themeName, string returnUrl = "")
        //{
        //    _themeContext.WorkingThemeName = themeName;

        //    //home page
        //    if (string.IsNullOrEmpty(returnUrl))
        //        returnUrl = Url.RouteUrl("HomePage");

        //    //prevent open redirection attack
        //    if (!Url.IsLocalUrl(returnUrl))
        //        returnUrl = Url.RouteUrl("HomePage");

        //    return Redirect(returnUrl);
        //}

        //[HttpPost]
        ////available even when a store is closed
        //[CheckAccessClosedStore(true)]
        ////available even when navigation is not allowed
        //[CheckAccessPublicStore(true)]
        //public virtual IActionResult EuCookieLawAccept()
        //{
        //    if (!_storeInformationSettings.DisplayEuCookieLawWarning)
        //        //disabled
        //        return Json(new { stored = false });

        //    //save setting
        //    _genericAttributeService.SaveAttribute(_workContext.CurrentCustomer, NopCustomerDefaults.EuCookieLawAcceptedAttribute, true, _storeContext.CurrentStore.Id);
        //    return Json(new { stored = true });
        //}

        //public virtual IActionResult GenericUrl()
        //{
        //    //seems that no entity was found
        //    return InvokeHttp404();
        //}

        ////store is closed
        ////available even when a store is closed
        //[CheckAccessClosedStore(true)]
        //public virtual IActionResult StoreClosed()
        //{
        //    return View();
        //}

        ////helper method to redirect users. Workaround for GenericPathRoute class where we're not allowed to do it
        //public virtual IActionResult InternalRedirect(string url, bool permanentRedirect)
        //{
        //    //ensure it's invoked from our GenericPathRoute class
        //    if (HttpContext.Items["nop.RedirectFromGenericPathRoute"] == null ||
        //        !Convert.ToBoolean(HttpContext.Items["nop.RedirectFromGenericPathRoute"]))
        //    {
        //        url = Url.RouteUrl("HomePage");
        //        permanentRedirect = false;
        //    }

        //    //home page
        //    if (string.IsNullOrEmpty(url))
        //    {
        //        url = Url.RouteUrl("HomePage");
        //        permanentRedirect = false;
        //    }

        //    //prevent open redirection attack
        //    if (!Url.IsLocalUrl(url))
        //    {
        //        url = Url.RouteUrl("HomePage");
        //        permanentRedirect = false;
        //    }

        //    if (permanentRedirect)
        //        return RedirectPermanent(url);

        //    return Redirect(url);
        //}

        #endregion
    }
}