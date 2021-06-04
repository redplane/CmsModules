using System;
using CmsModules.ManagementUi.Enums;
using CmsModules.ManagementUi.ViewModels;
using CorsModule.Models.Interfaces;
using MediatR;

namespace CmsModules.ManagementUi.Cqrs.Commands.CorsPolicies
{
    public class UpdateCorsPolicyCommand : IRequest<ICorsPolicy>
    {
        #region Properties
        
        public Guid Id { get; set; }

        public EditableFieldViewModel<string[]> AllowedHeaders { get; set; }

        public EditableFieldViewModel<string[]> AllowedOrigins { get; set; }

        public EditableFieldViewModel<string[]> AllowedMethods { get; set; }

        public EditableFieldViewModel<string[]> AllowedExposedHeaders { get; set; }

        public EditableFieldViewModel<bool> AllowCredential { get; set; }

        public EditableFieldViewModel<MasterItemAvailabilities> Availability { get; set; }

        public EditableFieldViewModel<Guid?> SiteId { get; set; }

        #endregion
    }
}
