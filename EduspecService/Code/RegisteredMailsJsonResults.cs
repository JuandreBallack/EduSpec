﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SearchResult
{
    public partial class RegEmailSearchResult
    {
        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("result_count")]
        public long ResultCount { get; set; }

        [JsonProperty("results")]
        public Result[] Results { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("email_to")]
        public string EmailTo { get; set; }

        [JsonProperty("email_from")]
        public string EmailFrom { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("date_sent")]
        public DateTime? DateSent { get; set; }

        [JsonProperty("date_delivered")]
        public DateTime? DateDelivered { get; set; }

        [JsonProperty("response")]
        public string Response { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("certificate_processed")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long CertificateProcessed { get; set; }

        [JsonProperty("certificate_url")]
        public Uri CertificateUrl { get; set; }
    }

    public partial class RegEmailSearchResult
    {
        public static RegEmailSearchResult FromJson(string json) => JsonConvert.DeserializeObject<RegEmailSearchResult>(json, SearchResult.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this RegEmailSearchResult self) => JsonConvert.SerializeObject(self, SearchResult.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
