namespace CmsModulesManagement.ViewModels
{
    public class EditableFieldViewModel<T>
    {
        #region Properties

        public T Value { get; set; }

        public bool HasModified { get; set; }

        #endregion
    }
}