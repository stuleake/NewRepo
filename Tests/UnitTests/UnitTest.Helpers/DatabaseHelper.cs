using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TQ.Core.Constants;
using TQ.Core.Enums;
using TQ.Data.FormEngine;
using TQ.Data.FormEngine.Schemas.Forms;

namespace UnitTest.Helpers
{
    /// <summary>
    /// Database Helper Extension Methods
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class DatabaseHelper
    {
        // Instantiate an identity value holder
        private static int identity;

        private static int NextId => ++identity;

        /// <summary>
        /// <see cref="FormsEngineContext"/> extension method to create Field record
        /// </summary>
        /// <param name="formsEngineContext">instance of <see cref="FormsEngineContext"/> being extended</param>
        /// <param name="fieldType"><see cref="FieldTypes"/> for the field to create</param>
        /// <returns>copy of created <see cref="Field"/> record</returns>
        public static Field AddField(this FormsEngineContext formsEngineContext, FieldTypes fieldType)
        {
            var field = new Field
            {
                FieldId = Guid.NewGuid(),
                FieldTypeId = (int)fieldType
            };
            formsEngineContext?.Field.Add(field);

            return field;
        }

        /// <summary>
        /// <see cref="FormsEngineContext"/> extension method to create <see cref="SectionType"/> record
        /// </summary>
        /// <param name="formsEngineContext">instance of <see cref="FormsEngineContext"/> being extended</param>
        /// <returns>copy of created <see cref="SectionType"/> record</returns>
        public static SectionType AddSectionTypes(this FormsEngineContext formsEngineContext)
        {
            var id = NextId;
            var record = new SectionType
            {
                SectionTypeId = id,
                SectionTypes = $"SectionTypes {id}"
            };
            formsEngineContext?.SectionTypes.Add(record);

            return record;
        }

        /// <summary>
        /// <see cref="FormsEngineContext"/> extension method to create <see cref="QS"/> record
        /// </summary>
        /// <param name="formsEngineContext">instance of <see cref="FormsEngineContext"/> being extended</param>
        /// <param name="qsno">Question number for the <see cref="QS"/> record being created</param>
        /// <param name="version">QSVersion for the <see cref="QS"/> record being created</param>
        /// <param name="statusId">StatusId for the <see cref="QS"/> record being created</param>
        /// <returns>copy of created <see cref="QS"/> record</returns>
        public static QS AddQS(this FormsEngineContext formsEngineContext, int qsno, decimal version = 1.0M, int statusId = 2)
        {
            int id = NextId;
            var qs = new QS
            {
                QSId = Guid.NewGuid(),
                QSNo = qsno,
                Label = $"QS Label {id}",
                Description = $"QS Description {id}",
                Helptext = $"QS HelpText {id}",
                QSVersion = version,
                StatusId = statusId
            };
            formsEngineContext?.QS.Add(qs);

            return qs;
        }

        /// <summary>
        /// <see cref="FormsEngineContext"/> extension method to create <see cref="QSCollectionMapping"/> record
        /// </summary>
        /// <param name="formsEngineContext">instance of <see cref="FormsEngineContext"/> being extended</param>
        /// <param name="questionSetNumber">QSNo for the <see cref="QSCollectionMapping"/> record being created</param>
        /// <param name="questionSetId">QuestionSetId for the <see cref="QSCollectionMapping"/> record being created</param>
        /// <param name="questionSetCollectionTypeId">QSCollectionTypeId for the <see cref="QSCollectionMapping"/> record being created</param>
        /// <param name="sequence">Sequence for the <see cref="QSCollectionMapping"/> record being created</param>
        /// <returns>copy of created <see cref="QSCollectionMapping"/> record</returns>
        public static QSCollectionMapping AddQSCollectionMapping(
            this FormsEngineContext formsEngineContext,
            int questionSetNumber,
            Guid questionSetId,
            Guid questionSetCollectionTypeId,
            int sequence = 1)
        {
            var questionSetCollectionMapping = new QSCollectionMapping
            {
                QSCollectionMappingId = Guid.NewGuid(),
                QSCollectionTypeId = questionSetCollectionTypeId,
                QSId = questionSetId,
                QSNo = questionSetNumber,
                Sequence = sequence
            };
            formsEngineContext?.QSCollectionMapping.Add(questionSetCollectionMapping);

            return questionSetCollectionMapping;
        }

        /// <summary>
        /// <see cref="FormsEngineContext"/> extension method to create QSCollectionType record
        /// </summary>
        /// <param name="formsEngineContext">instance of <see cref="FormsEngineContext"/> being extended</param>
        /// <param name="country">country for the <see cref="QSCollectionType"/> record being created</param>
        /// <returns>copy of created <see cref="QSCollectionType"/> record</returns>
        public static QSCollectionType AddQSCollectionType(this FormsEngineContext formsEngineContext, string country)
        {
            var countries = new List<string> { CountryConstants.England.ToLower(), CountryConstants.Wales.ToLower() };
            if (!countries.Contains(country?.ToLower()))
            {
                throw new ArgumentException("provided country value is not valid");
            }

            var id = NextId;
            var record = new QSCollectionType
            {
                QSCollectionTypeId = Guid.NewGuid(),
                Label = $"QSCollectionType Label {id}",
                Description = $"QSCollectionType Description {id}",
                Helptext = $"QSCollectionType HelpText {id}",
                CountryCode = country,
                Product = "PP2"
            };
            formsEngineContext?.QSCollectionType.Add(record);

            return record;
        }

        /// <summary>
        /// FormsEngineContext extension method to setup FieldTypes based on <see cref="FieldTypes"/> enum
        /// </summary>
        /// <param name="formsEngineContext">instance of <see cref="FormsEngineContext"/> being extended</param>
        public static void SetupFieldTypes(this FormsEngineContext formsEngineContext)
        {
            var fieldTypes = Enum.GetValues(typeof(FieldTypes)).Cast<FieldTypes>();
            foreach (var item in fieldTypes)
            {
                var fieldType = new FieldType
                {
                    FieldTypeId = (int)item,
                    FieldTypes = item.ToString()
                };
                formsEngineContext.FieldTypes.Add(fieldType);
            }
            formsEngineContext.SaveChanges();
        }
    }
}