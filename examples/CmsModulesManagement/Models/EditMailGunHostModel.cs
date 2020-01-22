using CmsModulesManagement.Models.Interfaces;
using CmsModulesManagement.ViewModels;

namespace CmsModulesManagement.Models
{
    public class EditMailGunHostModel : IEditMailHost
    {
        #region Properties

        public string Type { get; set; }

        public EditableFieldViewModel<string> ApiKey { get; set; }

        public EditableFieldViewModel<string> Domain { get; set; }

        #endregion
    }
}