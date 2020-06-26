namespace Api.FormEngine.Core.Constants
{
    /// <summary>
    /// Manage error message in excel sheet
    /// </summary>
    internal class ApplicationConstants
    {
        /// <summary>
        /// Qs number header
        /// </summary>
        public const string QsNumberHeader = "QSNo";

        /// <summary>
        /// Qs version header
        /// </summary>
        public const string QsVersionHeader = "QSVersion";

        /// <summary>
        /// Key header
        /// </summary>
        public const string KeyHeader = "Key";

        /// <summary>
        /// Field number not present
        /// </summary>
        public const string FieldNumberNotPresent = "Field no {0} is not present in Fields. Please check the excel sheet.";

        /// <summary>
        /// QS number not present
        /// </summary>
        public const string QSNumberNotPresent = "Question set no {0} is not present in QS. Please check the excel sheet.";

        /// <summary>
        /// Multiple question set found in single excel file error message
        /// </summary>
        public const string MultipleQuestionSetFound = "More than one Question set found in same excel sheet cannot proceed with this excel file";

        /// <summary>
        /// If value is not number error message
        /// </summary>
        public const string IsNotNumber = "Invalid {0} Number : {1}";

        /// <summary>
        /// Value is empty or null error message
        /// </summary>
        public const string EmptyOrNullValue = "{0} cannot be null or empty";

        /// <summary>
        /// Duplicate question set found error message
        /// </summary>
        public const string DuplicateFound = "{0} : {1} is duplicate. Please check excel sheet";

        /// <summary>
        /// Duplicate Field or section found error message
        /// </summary>
        public const string DuplicateFieldSectionFound = "{0} : {1} is duplicate in {2} : {3}. Please check excel sheet";

        /// <summary>
        /// Uploaded file is not excel error message
        /// </summary>
        public const string NonExecutableFileFound = "File Name : {0} is not excel file you cannot proceed with this.";

        /// <summary>
        /// Invalid section type error message
        /// </summary>
        public const string InvalidSectionType = "Invalid Section Type : {0} in Section Number : {1}";

        /// <summary>
        /// Invalid field type error message
        /// </summary>
        public const string InvalidFieldType = "Invalid Field Type : {0} in Field Number : {1}";

        /// <summary>
        /// Invalid field display error message
        /// </summary>
        public const string InvalidFieldDisplay = "Invalid Field Display : {0} in Field Number : {1}";

        /// <summary>
        /// If value is not bool show message
        /// </summary>
        public const string ValueIsNotBoolErrorMessage = "Value {0} is not boolean in {1} column";

        /// <summary>
        /// Answer guide error in input field type
        /// </summary>
        public const string FieldTypeInputErrorMessage = "Can't set date min, date max and API if field type is number. Field Number : {0}";

        /// <summary>
        /// Answer guide error in dropdown field type
        /// </summary>
        public const string FieldTypeDropdownErrorMessage = "Can't set range min, range max, length min, length max, date min, date max, multiple if field type is Dropdown/Radio. Field Number : {0}";

        /// <summary>
        /// Answer guide error in date field type
        /// </summary>
        public const string FieldTypeDateErrorMessage = "Can't set range min, range max, length min, length max, multiple, api if field type is Date. Field Number : {0}";

        /// <summary>
        /// Answer guide error in text field type
        /// </summary>
        public const string FieldTypeTextErrorMessage = "Can't set range min, range max, date min, date max, multiple if field type is Text. Field Number : {0}";

        /// <summary>
        /// Field not exits in question set {0} is column name {1} field number
        /// </summary>
        public const string FieldNotExistErrorMessage = "{0} : {1} does not exist in Question Set Sheet";

        /// <summary>
        /// Field not exits in question set {0} is column name {1} field number
        /// </summary>
        public const string DependsOnAnsDoesNotExist = "{0} : {1} does not exist in Answer Guide Sheet";

        /// <summary>
        /// DUplicate field number found in aggregation field number column
        /// </summary>
        public const string DuplicateAggregationFieldNumberFound = "Multiple Aggregation Field Number found in same Field Number : {0} cannot process this aggregation";

        /// <summary>
        /// Field number and aggregation number both are same error message
        /// </summary>
        public const string SameFieldNoAndAggregationNo = "Field Number {0} and aggregation number {0} both are same cannot process this aggregation";

        /// <summary>
        /// duplicate priority found
        /// </summary>
        public const string DuplicatePriorityFound = "Duplicate priority number found in field number : {0} cannot process this aggregation";

        /// <summary>
        /// Depends count is greater than number of depends on ans
        /// </summary>
        public const string DependCountMoreThanDependsOnAns = "Dependencies: Depends count: value of depends count is more than no of dependsOnAns for field: {0}";

        /// <summary>
        /// Duplicate depends on ans found
        /// </summary>
        public const string DuplicateDependsOnAnsFound = "Dependencies: Duplicate depends on ans found in field {0}";

        /// <summary>
        /// Duplicate decides section found
        /// </summary>
        public const string DuplicateDecidesSectionFound = "Dependencies: Duplicate decides section found in field {0}";

        /// <summary>
        /// Both field and section cannot be empty
        /// </summary>
        public const string BothFieldAndSectionEmpty = "Both field and section cannot be empty";

        /// <summary>
        /// Both field and section cannot be empty
        /// </summary>
        public const string BothFieldAndSectionIsNotEmpty = "Both Field Number and Section Number cannot be set at the same time";

        /// <summary>
        /// Field type is not number error message
        /// </summary>
        public const string FieldTypeNotNumber = "Field type : {0} is not Number, cannot process this aggregation. Field Number : {1}";

        /// <summary>
        /// Field no and Copy from Qs not found in dependency
        /// </summary>
        public const string FieldAndCopyFromQsNotExists = "Field no : {0} and Copy from Qs : {1}, not found in dependencies";

        /// <summary>
        /// aggregation field type in not number error message
        /// </summary>
        public const string AggregationFieldTypeNotNumber = "Aggregation Field type : {0} is not number, cannot process this aggregation. Aggregation Field Number : {1}";

        /// <summary>
        /// Aggregation error message
        /// </summary>
        public const string CanNotProcessAggregation = "Cannot process this aggregation";

        /// <summary>
        /// invalid function
        /// </summary>
        public const string InvalidFunction = "Invalid function : {0}";

        /// <summary>
        /// minimum date is not number error
        /// </summary>
        public const string MinDateNotNumberError = "Date Min is not a number or today";

        /// <summary>
        /// maximum date is not number error
        /// </summary>
        public const string MaxDateNotNumberError = "Date Max is not a number or today";

        /// <summary>
        /// Invalid section rule error message
        /// </summary>
        public const string InvalidSectionRule = "Invalid section rule found. Section rule : {0}";

        /// <summary>
        /// check min value is less than max value
        /// </summary>
        public const string MinValueShouldNotGreaterThanMaxValue = "{0} min should not be greater that {0} max. Field number {1}";

        /// <summary>
        /// field string
        /// </summary>
        public const string Field = "Field";

        /// <summary>
        /// copy from qs
        /// </summary>
        public const string CopyFromQS = "Copy from QS";

        /// <summary>
        /// copy from field
        /// </summary>
        public const string CopyFromField = "Copy from Field";

        /// <summary>
        /// ToBeRedacted
        /// </summary>
        public const string ToBeRedacted = "ToBeRedacted";

        /// <summary>
        /// answer string
        /// </summary>
        public const string Answer = "Answer";

        /// <summary>
        /// range minimum string
        /// </summary>
        public const string RangeMin = "Range Minimum";

        /// <summary>
        /// range maximum string
        /// </summary>
        public const string RangeMax = "Range Maximum";

        /// <summary>
        /// Length minimum string
        /// </summary>
        public const string LengthMin = "Length Minimum";

        /// <summary>
        /// Length maximum string
        /// </summary>
        public const string LengthMax = "Length Maximum";

        /// <summary>
        /// multiple string
        /// </summary>
        public const string Multiple = "Multiple";

        /// <summary>
        /// IsDefault string
        /// </summary>
        public const string IsDefault = "IsDefault";

        /// <summary>
        /// today string
        /// </summary>
        public const string Today = "today";

        /// <summary>
        /// depends on ans string
        /// </summary>
        public const string DependsOnAns = "Depends On Ans";

        /// <summary>
        /// Depends Count string
        /// </summary>
        public const string DependsCount = "Depends Count";

        /// <summary>
        /// Decides section string
        /// </summary>
        public const string DecidesSection = "Decides section";

        /// <summary>
        /// dependencies string
        /// </summary>
        public const string Dependencies = "Dependencies:";

        /// <summary>
        /// no question set found string
        /// </summary>
        public const string NoQuestionSetFound = "No question set found";

        /// <summary>
        /// Question Set string
        /// </summary>
        public const string QuestionSet = "Question Set";

        /// <summary>
        /// Question number string
        /// </summary>
        public const string QuestionNo = "Question No";

        /// <summary>
        /// Section Number string
        /// </summary>
        public const string SectionNo = "Section No";

        /// <summary>
        /// section rule count string
        /// </summary>
        public const string SectionRuleCount = "Section Rule Count";

        /// <summary>
        /// section string
        /// </summary>
        public const string Section = "Section";

        /// <summary>
        /// range string
        /// </summary>
        public const string Range = "Range";

        /// <summary>
        /// Length string
        /// </summary>
        public const string Length = "Length";

        /// <summary>
        /// Number string
        /// </summary>
        public const string NUMBER = "number";

        /// <summary>
        /// field number string
        /// </summary>
        public const string FieldNumber = "Field No";

        /// <summary>
        /// Aggregation Field Number string
        /// </summary>
        public const string AggregationFieldNumber = "Aggregation Field Number";

        /// <summary>
        /// Priority string
        /// </summary>
        public const string Priority = "Priority";

        /// <summary>
        /// Deleted field number message
        /// </summary>
        public const string DeletedFieldNumber = "Field number {0} deleted";

        /// <summary>
        /// deleted field using in other sheets error message
        /// </summary>
        public const string DeletedFieldInOtherSheets = "You are trying to create {0} of deleted field number {1}";

        /// <summary>
        /// Dependency on deleted field
        /// </summary>
        public const string DependencyOnDeletedField = "You are trying to create dependencies of deleted field number {0} or deleted answer guide number {1}";

        /// <summary>
        /// Incorrect update action error message
        /// </summary>
        public const string IncorrectUpdateAction = "Action of Field number {0} is Update not {1}";

        /// <summary>
        /// Incorrect add action error message
        /// </summary>
        public const string IncorrectAddAction = "Action of Field number {0} is Add not {1}";

        /// <summary>
        /// Field added message
        /// </summary>
        public const string FieldAdded = "Field number {0} added";

        /// <summary>
        /// Field type update message
        /// </summary>
        public const string FieldTypeUpdateMessage = "Field number {0} updated form Field Type {1} to Field Type {2}";

        /// <summary>
        /// Update
        /// </summary>
        public const string Update = "Update";

        /// <summary>
        /// Delete
        /// </summary>
        public const string Delete = "Delete";

        /// <summary>
        /// Add
        /// </summary>
        public const string Add = "Add";

        /// <summary>
        /// Metadatas
        /// </summary>
        public const string Metadatas = "Metadatas";

        /// <summary>
        /// ContainerName
        /// </summary>
        public const string ContainerName = "ContainerName";

        /// <summary>
        /// SubContainerName
        /// </summary>
        public const string SubContainerName = "SubContainerName";

        /// <summary>
        /// Documents
        /// </summary>
        public const string Documents = "Documents";

        /// <summary>
        /// Description
        /// </summary>
        public const string Description = "Description";

        /// <summary>
        /// blob file description
        /// </summary>
        public const string BlobFileDescription = "This is {0}";
    }
}