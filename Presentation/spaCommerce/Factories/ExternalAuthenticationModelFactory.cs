﻿using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Services.Authentication.External;
using spaCommerce.Models.Customer;

namespace spaCommerce.Factories
{
    /// <summary>
    /// Represents the external authentication model factory
    /// </summary>
    public partial class ExternalAuthenticationModelFactory : IExternalAuthenticationModelFactory
    {
        #region Fields

        private readonly IExternalAuthenticationService _externalAuthenticationService;
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public ExternalAuthenticationModelFactory(IExternalAuthenticationService externalAuthenticationService,
            IStoreContext storeContext,
            IWorkContext workContext)
        {
            this._externalAuthenticationService = externalAuthenticationService;
            this._storeContext = storeContext;
            this._workContext = workContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare the external authentication method model
        /// </summary>
        /// <returns>List of the external authentication method model</returns>
        public virtual List<ExternalAuthenticationMethodModel> PrepareExternalMethodsModel()
        {
            return _externalAuthenticationService
                .LoadActiveExternalAuthenticationMethods(_workContext.CurrentCustomer, _storeContext.CurrentStore.Id)
                .Select(authenticationMethod => new ExternalAuthenticationMethodModel
                {
                    ViewComponentType = authenticationMethod.GetPublicViewComponentName()
                }).ToList();
        }

        #endregion
    }
}
