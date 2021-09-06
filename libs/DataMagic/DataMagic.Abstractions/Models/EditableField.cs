using DataMagic.Abstractions.Interfaces;

namespace DataMagic.Abstractions.Models
{
    public class EditableField<T> : IEditableField<T>
    {
        #region Properties

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        public T Value { get; private set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        public bool HasModified { get; private set; }

        #endregion

        #region Constructor

        public EditableField(T value)
        {
            Value = value;
            HasModified = true;
        }

        public EditableField()
        {
            HasModified = false;
        }

        #endregion
    }
}