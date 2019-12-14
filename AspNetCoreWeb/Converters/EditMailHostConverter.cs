using System;
using System.Linq;
using System.Reflection;
using AspNetWebShared.Constants;
using MailWeb.Constants;
using MailWeb.Models;
using MailWeb.Models.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MailWeb.Converters
{
    public class EditMailHostConverter : JsonConverter
    {
        #region Properties

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var mailHost = value as IEditMailHost;
            var type = ToEditMailHostType(mailHost?.Type);
            serializer.Serialize(writer, mailHost, type);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jToken = JToken.ReadFrom(reader);
            var editMailHostTypeToken = jToken.SelectToken("type");
            if (editMailHostTypeToken == null)
                return default;

            var mailHostTypeName = editMailHostTypeToken.Value<string>();
            var mailHostType = ToEditMailHostType(mailHostTypeName);
            return ToMailHost(jToken, mailHostType);
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        protected virtual IEditMailHost ToMailHost(JToken jToken, Type type)
        {
            var mi = typeof(JToken)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(m => m.Name == "ToObject" && m.GetParameters().Length == 0 && m.IsGenericMethod)
                ?.MakeGenericMethod(type);

            var a = mi?.Invoke(jToken, null);
            return (IEditMailHost) a;
        }

        protected virtual Type ToEditMailHostType(string typeName)
        {
            switch (typeName)
            {
                case MailHostKindConstants.Smtp:
                    return typeof(EditSmtpHostModel);

                case MailHostKindConstants.MailGun:
                    return typeof(EditMailGunHostModel);
            }

            throw new Exception("Argument not supported.");
        }

        #endregion
    }
}