using System;

namespace MailWeb.Models.ValueObjects
{
    public class MailServiceValueObject
    {
        #region Properties

        public string Name { get; }

        public string Type { get; }

        #endregion

        #region Constructor

        public MailServiceValueObject(string name, string type)
        {
            Name = name;
            Type = type;
        }

        public MailServiceValueObject(string name, Type type)
        {
            Name = name;
            Type = type.FullName;
        }

        #endregion
    }
}