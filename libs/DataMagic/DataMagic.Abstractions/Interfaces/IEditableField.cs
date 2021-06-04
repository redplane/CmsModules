namespace DataMagic.Abstractions.Interfaces
{
    public interface IEditableField<out T>
    {
        #region Properties
        
        T Value { get; }

        bool HasModified { get; }

        #endregion
    }
}
