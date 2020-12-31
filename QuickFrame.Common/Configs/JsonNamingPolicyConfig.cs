using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuickFrame.Common
{
    /// <summary>
    /// JSON属性序列化策略
    /// </summary>
    public class JsonNamingPolicyConfig
    {
        /// <summary>
        /// 小写
        /// </summary>
        public static JsonNamingPolicy LowerCase { get; } = new LowerCasePolicy();
        /// <summary>
        /// 驼峰
        /// </summary>
        public static JsonNamingPolicy CamelCase { get; } = JsonNamingPolicy.CamelCase;
        /// <summary>
        /// 大写
        /// </summary>
        public static JsonNamingPolicy UpperCase { get; } = new UpperCasePolicy();
    }
    /// <summary>
    /// 序列化时属性转为小写
    /// </summary>
    public class LowerCasePolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name.ToLower();
        }
    }
    /// <summary>
    /// 序列化时属性转为大写
    /// </summary>
    public class UpperCasePolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name.ToUpper();
        }
    }
    /// <summary>
    /// 元组序列化转换器
    /// </summary>
    public class TupleJsonConverter : JsonConverter<ITuple>
    {
        public override ITuple? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();//未实现元组的反序列化
            //reader.Read();
            //_ = reader.TokenType == JsonTokenType.StartObject ? true : throw new ArgumentException("元组类型反序列化发生错误");
            //while (reader.Read())
            //{
            //    if (reader.TokenType == JsonTokenType.PropertyName) continue;
            //    if (reader.TokenType == JsonTokenType.EndObject) break;

            //    var name = reader.GetString();
            //    ValueTuple.Create();
            //}
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsAssignableTo(typeof(ITuple));
        }

        public override void Write(Utf8JsonWriter writer, ITuple value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            for (int i = 0; i < value.Length; i++)
            {
                writer.WritePropertyName($"part{i}");
                switch (value[i])
                {
                    case int item:
                        writer.WriteNumberValue(item);
                        break;
                    case uint item:
                        writer.WriteNumberValue(item);
                        break;
                    case double item:
                        writer.WriteNumberValue(item);
                        break;
                    case float item:
                        writer.WriteNumberValue(item);
                        break;
                    case decimal item:
                        writer.WriteNumberValue(item);
                        break;
                    case long item:
                        writer.WriteNumberValue(item);
                        break;
                    case ulong item:
                        writer.WriteNumberValue(item);
                        break;
                    case bool item:
                        writer.WriteBooleanValue(item);
                        break;
                    case string item:
                        writer.WriteStringValue(item);
                        break;
                    default:
                        writer.WriteStringValue(value[i]?.ToString());
                        break;
                }
            }
            writer.WriteEndObject();
        }
    }
}