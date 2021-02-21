using System;
using System.Collections.Generic;
using System.Text;

namespace TRoschinsky.SPDataModel.Lib.ModelGenerators
{
    public class SpJsomInstaller : ModelGenerator
    {

        private const string genJsHead = "// install a data model\n" +
            "let oInstData = new SpInstallerData();\n";
        private const string genJsEntity = "oInstData.addList(\"{0}\", \"{1}\", [\n{2}]\n);";
        // Field templates (see https://docs.microsoft.com/de-de/sharepoint/dev/schema/field-element-field)
        // 0:name, 1:displayName, 2:desc, 3:required, 4:addToView, 5:type, 6:attributes, 7:elementContents
        private const string genJsField = "    {{ fieldName: \"{0}\", fieldDesc: \"{2}\", fieldXml: \"<Field Type='{5}' DisplayName='{1}' Name='{0}' StaticName='{0}' Required='{3}' {6}>{7}</Field>\", addToView: {4} }},\n";


        public SpJsomInstaller(Model inputModel) : base(inputModel)
        {
            RelatedApplication = new Uri("https://www.microsoft.com/de-de/microsoft-365/sharepoint/collaboration");
            RelatedApplicationUsage = "Upload the installer script to your SharePoint webs SiteAssets and SitePages libraries and run the 'Installer.aspx'.";
            OutputType = typeof(string);
            Generate();
        }

        public override void Generate()
        {
            StringBuilder outputSb = new StringBuilder();
            foreach (Entity entity in Input.Entities)
            {
                try
                {
                    if ((entity.IsHidden && Settings.ShowHiddenLists) || (!entity.IsHidden && !entity.IsSystem))
                    {
                        StringBuilder outputSbFields = new StringBuilder();
                        List<Field> fieldList = new List<Field>();
                        fieldList.AddRange(entity.Fields);

                        foreach (Field field in fieldList.FindAll(f => !(f.IsHidden || f.IsSystem)))
                        {
                            string fieldSpecialAttributes = String.Empty;
                            string fieldSpecialContent = String.Empty;

                            switch (field.FieldType)
                            {
                                case Field.TypeOfField.Boolean:
                                    fieldSpecialContent = "<Default>0</Default>";
                                    break;
                                
                                case Field.TypeOfField.DateTime:
                                    var fieldDate = field as FieldTypes.FieldDateTime;
                                    fieldSpecialAttributes = String.Format("Format='{0}' ", 
                                        fieldDate.ShowDateAndTime ? "DateTime" : "DateOnly");
                                    break;
                                
                                case Field.TypeOfField.Url:
                                    var fieldUrl = field as FieldTypes.FieldUrl;
                                    fieldSpecialAttributes = String.Format("Format='{0}' ",fieldUrl.Format);
                                    break;
                                
                                case Field.TypeOfField.Number:
                                    var fieldNumber = field as FieldTypes.FieldNumber;
                                    fieldSpecialAttributes = String.Format("Decimals='{0}' {1}{2}{3}", 
                                        fieldNumber.Decimals,
                                        fieldNumber.ValueMinimum != int.MinValue ? String.Format("Min='{0}' ", fieldNumber.ValueMinimum) : String.Empty,
                                        fieldNumber.ValueMaximum != int.MaxValue ? String.Format("Max='{0}' ", fieldNumber.ValueMaximum) : String.Empty,
                                        fieldNumber.AsPercentage ? "" : String.Empty);
                                    break;
                                    
                                case Field.TypeOfField.Note:
                                    var fieldNote = field as FieldTypes.FieldMultiLineText;
                                    fieldSpecialAttributes = String.Format("RichText='{0}' {1}{2}NumLines='{3}' AppendOnly='{4}' ", 
                                        fieldNote.IsRichTextEnabled ? "TRUE" : "FALSE",
                                        fieldNote.IsRichTextEnabled ? String.Format("RichTextMode='{0}' ", fieldNote.RichTextMode) : String.Empty,
                                        fieldNote.IsRichTextEnabled ? String.Format("IsolateStyles='{0}' ", fieldNote.IsRichTextIsolatedStyles ? "TRUE" : "FALSE") : String.Empty,
                                        fieldNote.NumLines, 
                                        fieldNote.AppendOnly ? "TRUE" : "FALSE");
                                    break;

                                case Field.TypeOfField.Lookup:
                                    var fieldLookup = field as FieldTypes.FieldLookup;
                                    fieldSpecialAttributes = String.Format("List='{0}' ShowField='{1}' RelationshipDeleteBehavior='{2}' Mult='{3}' ", 
                                        fieldLookup.List, 
                                        fieldLookup.ShowField, 
                                        fieldLookup.RelationshipDeleteBehavior, 
                                        fieldLookup.IsMultiLookup);
                                    break;

                                case Field.TypeOfField.Choice:
                                case Field.TypeOfField.MultiChoice:
                                    var fieldChoice = field as FieldTypes.FieldChoice;
                                    if (fieldChoice == null)
                                    {
                                        fieldChoice = field as FieldTypes.FieldMultiChoice;
                                    }

                                    fieldSpecialAttributes = String.Format("Format='{0}'", field.Format);
                                    foreach (string choice in fieldChoice.Choices)
                                    {
                                        fieldSpecialContent += "<CHOICE>" + choice.Trim() + "</CHOICE>";
                                    }
                                    fieldSpecialContent = String.Format("<CHOICES>{0}</CHOICES>", fieldSpecialContent);
                                    break;

                                case Field.TypeOfField.User:
                                    var fieldUser = field as FieldTypes.FieldUser;
                                    fieldSpecialAttributes = String.Format("UserSelectionMode='{0}' UserSelectionScope='{1}' Mult='{2}' ", 
                                        fieldUser.UserSelectionMode, 
                                        fieldUser.UserSelectionScope, 
                                        fieldUser.IsMultiLookup);
                                    break;

                            }

                            // 0:name, 1:displayName, 2:desc, 3:required, 4:addToView, 5:type, 6:attributes, 7:elementContents
                            string codeJsfield = String.Format(genJsField,
                                field.InternalName,
                                field.DisplayName,
                                field.Description,
                                field.IsRequiredField ? "TRUE" : "FALSE",
                                field.InitialAddToView ? "true" : "false",
                                field.FieldType,
                                fieldSpecialAttributes,
                                fieldSpecialContent);
                            outputSbFields.Append(codeJsfield);
                        }

                        outputSb.AppendLine(String.Format(genJsEntity, 
                            entity.InternalName, 
                            entity.Description, 
                            outputSbFields.ToString().TrimEnd('\n', ',')));
                    }
                }
                catch (Exception ex)
                {
                    outputSb.AppendLine(String.Format("// Error processing entity '{0}': {1}", entity.InternalName, ex.Message));
                }
            }

            Output = String.Concat(genJsHead, outputSb.ToString());
        }
    }
}
