using System;
using System.Linq;
using System.Reflection;
using MailWeb.Models.Interfaces;
using MailWeb.Models.ValueObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MailWeb.Converters
{
    public class MailHostConverter : JsonConverter
    {
        #region Methods

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var mailHost = value as IMailHost;
            serializer.Serialize(writer, mailHost);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jToken = JToken.ReadFrom(reader);
            var mailHostTypeToken = jToken.SelectToken("type");
            if (mailHostTypeToken == null) 
                return default;

            var mailHostTypeName = mailHostTypeToken.Value<string>();
            var mailHostType = Type.GetType(mailHostTypeName);
            return ToMailHost(jToken, mailHostType);
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        protected virtual IMailHost ToMailHost(JToken jToken, Type type)
        {
            var mi = typeof(JToken)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(m => m.Name == "ToObject" && m.GetParameters().Length == 0 && m.IsGenericMethod)
                ?.MakeGenericMethod(type);

            return (IMailHost) mi?.Invoke(jToken, null);
        }

        protected virtual Type ResolveMailHostType(string type)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}