﻿using Microsoft.AspNetCore.Mvc;
using spaCommerce.Factories;
using Nop.Web.Framework.Components;

namespace spaCommerce.Components
{
    public class ExternalMethodsViewComponent : NopViewComponent
    {
        #region Fields

        private readonly IExternalAuthenticationModelFactory _externalAuthenticationModelFactory;

        #endregion

        #region Ctor

        public ExternalMethodsViewComponent(IExternalAuthenticationModelFactory externalAuthenticationModelFactory)
        {
            this._externalAuthenticationModelFactory = externalAuthenticationModelFactory;
        }

        #endregion

        #region Methods

        public IViewComponentResult Invoke()
        {
            var model = _externalAuthenticationModelFactory.PrepareExternalMethodsModel();

            return View(model);
        }

        #endregion
    }
}
