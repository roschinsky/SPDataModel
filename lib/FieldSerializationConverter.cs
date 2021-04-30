using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using TRoschinsky.SPDataModel.Lib.FieldTypes;
using static TRoschinsky.SPDataModel.Lib.Field;

//FieldSerializationConverter

namespace TRoschinsky.SPDataModel.Lib
{
    public class FieldSerializationConverter : JsonConverter<Field>
    {
        private const string fieldTypeJsonName = "FieldType";

        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(Field).IsAssignableFrom(typeToConvert);
        }

        public override Field Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Dictionary<string, object> fieldData = new Dictionary<string, object>();

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("First element expected should be of type 'StartObject'.");
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Second element expected should be of type 'PropertyName'.");
            }

            string propertyName = reader.GetString();
            if (propertyName != fieldTypeJsonName)
            {
                throw new JsonException("Second element property name is expected to be 'FieldType'.");
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.Number)
            {
                throw new JsonException("Second element property value is expected to be a number.");
            }

            TypeOfField typeDiscriminator = (TypeOfField)reader.GetInt32();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    string displayName = fieldData["DisplayName"] as string;
                    string internalName = fieldData["InternalName"] as string;

                    Field field = typeDiscriminator switch
                    {
                        TypeOfField.Boolean => new FieldBoolean(displayName, internalName),
                        TypeOfField.Choice => new FieldChoice(displayName, internalName),
                        TypeOfField.MultiChoice => new FieldChoice(displayName, internalName),
                        TypeOfField.Text => new FieldText(displayName, internalName),
                        TypeOfField.Note => new FieldMultiLineText(displayName, internalName),
                        TypeOfField.DateTime => new FieldDateTime(displayName, internalName),
                        TypeOfField.Number => new FieldNumber(displayName, internalName),
                        TypeOfField.Lookup => new FieldLookup(displayName, internalName, String.Empty),
                        TypeOfField.User => new FieldUser(displayName, internalName, (fieldData.ContainsKey("UserSelectionMode") && ((int)fieldData["UserSelectionMode"] == 1)) ? true : false, ""),                        
                        TypeOfField.Url => new FieldUrl(displayName, internalName) { IsHyperlink = !fieldData.ContainsKey("IsHyperlink") || (bool)fieldData["IsHyperlink"] },
                        // TODO: Handle non relevant files
                        TypeOfField.System => new FieldText(displayName, internalName),
                        TypeOfField.Complex => new FieldText(displayName, internalName),
                        TypeOfField.File => new FieldText(displayName, internalName),
                        TypeOfField.Undefined => new FieldText(displayName, internalName),
                        _ => throw new JsonException("Second element property value is expected to match a supported field type.")
                    };

                    return field;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    propertyName = reader.GetString();
                    reader.Read();

                    if(propertyName.Equals("NumLines") || 
                        propertyName.Equals("UserSelectionMode") || 
                        propertyName.Equals("UserSelectionScope") || 
                        propertyName.Equals("Decimals") || 
                        propertyName.Equals("ValueMinimum") || 
                        propertyName.Equals("ValueMaximum")) 
                    {
                        fieldData.Add(propertyName, reader.GetInt32());
                    }
                    else if(propertyName.Equals("AppendOnly") || 
                        propertyName.Equals("IsRichTextEnabled") || 
                        propertyName.Equals("IsRichTextFullHtml") || 
                        propertyName.Equals("IsRichTextIsolatedStyles") || 
                        propertyName.Equals("IsMultiLookup") || 
                        propertyName.Equals("IsHyperlink") || 
                        propertyName.Equals("AsPercentage") || 
                        propertyName.Equals("ShowDateAndTime") || 
                        propertyName.Equals("ShowAsDropdown") || 
                        propertyName.Equals("FillInChoice") || 
                        propertyName.Equals("IsRequiredField") || 
                        propertyName.Equals("InitialAddToView") || 
                        propertyName.Equals("IsSystem") || 
                        propertyName.Equals("IsHidden")) 
                    {
                        fieldData.Add(propertyName, reader.GetBoolean());
                    }
                    else if(propertyName.Equals("RelationshipDeleteBehavior"))
                    {
                        // TODO: Handle RelationshipDeleteBehavior
                        fieldData.Add(propertyName, reader.GetBoolean());
                    }
                    else if(propertyName.Equals("Choices"))
                    {
                        // TODO: Handle choices
                    }
                    else if(reader.TokenType == JsonTokenType.StartObject) 
                    {
                        string objectName = propertyName;
                        Dictionary<string, object> relationProperties = new Dictionary<string, object>();

                        reader.Read();
                        propertyName = reader.GetString();
                        reader.Read();
                        relationProperties.Add(propertyName, reader.GetString());
                        reader.Read();
                        propertyName = reader.GetString();
                        reader.Read();
                        relationProperties.Add(propertyName, reader.GetString());
                        reader.Read();
                        propertyName = reader.GetString();
                        reader.Read();
                        relationProperties.Add(propertyName, reader.GetBoolean());

                        Relation relation = new Relation(
                            relationProperties["LookupFromEntityName"] as string, 
                            relationProperties["LookupToEntityName"] as string) { 
                            IsMultiLookup = (bool)relationProperties["IsMultiLookup"] };
                        fieldData.Add(objectName, relation);

                        while(reader.TokenType != JsonTokenType.EndObject)
                        {
                            reader.Read();
                        }
                    }
                    else 
                    {
                        fieldData.Add(propertyName, reader.GetString());
                    }
                }
            }

            throw new JsonException("Unexpected end of object.");
        }

        public override void Write(Utf8JsonWriter writer, Field field, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            switch (field.FieldType)
            {
                case TypeOfField.Boolean:
                    writer.WriteNumber(fieldTypeJsonName, (int)TypeOfField.Boolean);
                    break;

                case TypeOfField.MultiChoice:
                case TypeOfField.Choice:
                    writer.WriteNumber(fieldTypeJsonName, field.FieldType == TypeOfField.MultiChoice ? (int)TypeOfField.MultiChoice : (int)TypeOfField.Choice);
                    FieldChoice fieldChoice = field as FieldChoice;
                    writer.WriteStartArray("Choices");
                    foreach(string choice in ((FieldChoice)field).Choices) 
                    {
                        writer.WriteStringValue(choice);
                    }
                    writer.WriteEndArray();
                    writer.WriteBoolean("ShowAsDropdown", fieldChoice.ShowAsDropdown);
                    writer.WriteBoolean("FillInChoice", fieldChoice.FillInChoice);
                    break;

                case TypeOfField.Complex:
                    writer.WriteNumber(fieldTypeJsonName, (int)TypeOfField.Complex);
                    break;

                case TypeOfField.DateTime:
                    writer.WriteNumber(fieldTypeJsonName, (int)TypeOfField.DateTime);
                    FieldDateTime fieldDateTime = field as FieldDateTime;
                    writer.WriteBoolean("ShowDateAndTime", fieldDateTime.ShowDateAndTime);
                    break;

                case TypeOfField.File:
                    writer.WriteNumber(fieldTypeJsonName, (int)TypeOfField.File);
                    break;

                case TypeOfField.Lookup:
                    writer.WriteNumber(fieldTypeJsonName, (int)TypeOfField.Lookup);
                    break;

                case TypeOfField.Note:
                    writer.WriteNumber(fieldTypeJsonName, (int)TypeOfField.Note);
                    break;

                case TypeOfField.Number:
                    writer.WriteNumber(fieldTypeJsonName, (int)TypeOfField.Number);
                    break;

                case TypeOfField.System:
                    writer.WriteNumber(fieldTypeJsonName, (int)TypeOfField.System);
                    break;

                case TypeOfField.Text:
                    writer.WriteNumber(fieldTypeJsonName, (int)TypeOfField.Text);
                    break;

                case TypeOfField.Url:
                    writer.WriteNumber(fieldTypeJsonName, (int)TypeOfField.Url);
                    break;

                case TypeOfField.User:
                    writer.WriteNumber(fieldTypeJsonName, (int)TypeOfField.User);
                    break;

                default:
                    writer.WriteNumber(fieldTypeJsonName, (int)TypeOfField.Undefined);
                    break;
            }

            writer.WriteString("DisplayName", field.DisplayName);
            writer.WriteString("InternalName", field.InternalName);
            writer.WriteString("FieldTypeName", field.FieldTypeName);
            writer.WriteBoolean("IsHidden", field.IsHidden);
            writer.WriteBoolean("IsSystem", field.IsSystem);
            if (field.RelatedTo != null)
            {
                writer.WriteStartObject("RelatedTo");
                writer.WriteString("LookupFromEntityName", field.RelatedTo.LookupFromEntityName);
                writer.WriteString("LookupToEntityName", field.RelatedTo.LookupToEntityName);
                writer.WriteBoolean("IsMultiLookup", field.RelatedTo.IsMultiLookup);
                writer.WriteEndObject();
            }
            writer.WriteString("Description", field.Description);
            writer.WriteBoolean("IsRequiredField", field.IsRequiredField);
            writer.WriteBoolean("InitialAddToView", field.IsSystem);
            writer.WriteString("Format", field.Format);
            writer.WriteString("Default", field.Default);

            writer.WriteEndObject();
        }

    }

}