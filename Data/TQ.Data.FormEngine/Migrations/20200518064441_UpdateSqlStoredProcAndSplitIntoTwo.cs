using Microsoft.EntityFrameworkCore.Migrations;

namespace TQ.Data.FormEngine.Migrations
{
    public partial class UpdateSqlStoredProcAndSplitIntoTwo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var questionSetByIdSql = @"ALTER PROCEDURE [dbo].[GetQuestionSetbyId] @QuestionSetId UNIQUEIDENTIFIER
                                     AS
                                         BEGIN
                                             SELECT
                                                     QS.QSId,
                                                     QS.QSNo,
                                                     QS.QSVersion,
                                                     QS.QSName,
                                                     QS.Label,
                                                     QS.Helptext,
                                                     QS.[Description],
                                                     (
                                                         SELECT
                                                             [Status]
                                                         FROM
                                                             [forms].[Status] s
                                                         WHERE
                                                             s.StatusId = QS.StatusId
                                                     )                               AS [Status],
                                                     QS.CreatedDate,
                                                     QS.LastModifiedDate,
                                                     QS.LastModifiedBy,
                                                     QSSectionMappings.[Sequence]    AS SectionSequence,
                                                     Sections.SectionId,
                                                     Sections.Label                  AS SectionLabel,
                                                     Sections.Helptext               AS SectionHelpText,
                                                     Sections.[Description]          AS SectionDescription,
                                                     (
                                                         SELECT TOP 1
                                                                SectionTypes
                                                         FROM
                                                                [forms].SectionTypes
                                                         WHERE
                                                                SectionTypes.SectionTypeId = Sections.SectionTypeId
                                                     )                               AS SectionType,
                                                     (
                                                         SELECT TOP 1
                                                                [Rules]
                                                         FROM
                                                                [forms].Rules
                                                         WHERE
                                                                Rules.RuleId = Sections.RuleId
                                                     )                               AS SectionRule,
                                                     Sections.RuleCount              AS SectionRuleCount,
                                                     SectionFieldMappings.[Sequence] AS FieldSequence,
                                                     Fields.FieldId,
                                                     Fields.FieldNo,
                                                     Fields.FieldVersion,
                                                     Fields.Label                    AS FieldLabel,
                                                     Fields.Helptext                 AS FieldHelptext,
                                                     Fields.Description              AS FieldDescription,
                                                     (
                                                         SELECT
                                                             FieldTypes
                                                         FROM
                                                             [forms].FieldTypes FT
                                                         WHERE
                                                             FT.FieldTypeId = Fields.FieldTypeId
                                                     )                               AS FieldType,
                                                     (
                                                         SELECT
                                                             Displays
                                                         FROM
                                                             [forms].Displays
                                                         WHERE
                                                             Displays.DisplayId = Fields.DisplayId
                                                     )                               AS Display,
                                                     (
                                                         SELECT
                                                             [Constraints]
                                                         FROM
                                                             [forms].Constraints C
                                                         WHERE
                                                             C.ConstraintId = Fields.DisplayConstraintId
                                                     )                               AS DisplayConstraint,
                                                     (
                                                         SELECT
                                                             [Rules]
                                                         FROM
                                                             [forms].Rules R
                                                         WHERE
                                                             R.RuleId = Fields.ConstraintRuleId
                                                     )                               AS ConstraintRule,
                                                     Fields.ConstraintRuleCount,
                                                     (
                                                         SELECT
                                                             [Rules]
                                                         FROM
                                                             [forms].Rules RU
                                                         WHERE
                                                             RU.RuleId = Fields.AnswerRuleId
                                                     )                               AS AnswerRule,
                                                     Fields.AnswerRuleCount,
                                                     AnswerGuides.AnswerGuideId,
                                                     (
                                                         SELECT
                                                             LOWER(AnswerTypes)
                                                         FROM
                                                             [forms].AnswerTypes ATS
                                                         WHERE
                                                             ATS.AnswerTypeId = AnswerGuides.AnswerTypeId
                                                     )                               AS AnswerType,
                                                     AnswerGuides.ErrLabel           AS AnswerGuideError,
                                                     (
                                                         SELECT
                                                             CASE
                                                                 WHEN AnswerGuides.AnswerTypeId = 8
                                                                      AND AnswerGuides.[Min] IS NOT NULL
                                                                      AND AnswerGuides.[Min] <> ''
                                                                      AND AnswerGuides.[Min] <> 'today'
                                                                      AND ISDATE(AnswerGuides.[Min]) <> 1
                                                                     THEN
                                                                     CONCAT(   AnswerGuides.[Min], '-',
                                                                         (
                                                                             SELECT
                                                                                 FieldNo
                                                                             FROM
                                                                                 forms.Fields
                                                                             WHERE
                                                                                 FieldId = CONVERT(UNIQUEIDENTIFIER, AnswerGuides.[Min])
                                                                         )
                                                                           )
                                                                 ELSE
                                                                     AnswerGuides.[Min]
                                                             END
                                                     )                               AS [min],
                                                     (
                                                         SELECT
                                                             CASE
                                                                 WHEN AnswerGuides.AnswerTypeId = 8
                                                                      AND AnswerGuides.[Max] IS NOT NULL
                                                                      AND AnswerGuides.[Max] <> ''
                                                                      AND AnswerGuides.[Max] <> 'today'
                                                                      AND ISDATE(AnswerGuides.[Max]) <> 1
                                                                     THEN
                                                                     CONCAT(   AnswerGuides.[Max], '-',
                                                                         (
                                                                             SELECT
                                                                                 FieldNo
                                                                             FROM
                                                                                 forms.Fields
                                                                             WHERE
                                                                                 FieldId = CONVERT(UNIQUEIDENTIFIER, AnswerGuides.[Max])
                                                                         )
                                                                           )
                                                                 ELSE
                                                                     AnswerGuides.[Max]
                                                             END
                                                     )                               AS [max],
                                                     AnswerGuides.Label              AS AnswerGuideLabel,
                                                     (
                                                         SELECT
                                                             CASE
                                                                 WHEN AnswerGuides.AnswerTypeId <> 3
                                                                     THEN
                                                                     AnswerGuides.[Value]
                                                                 ELSE
                                                                     NULL
                                                             END
                                                     )                               AS AnswerGuideValue,
                                                     AnswerGuides.IsDefault,
                                                     AnswerGuides.[Sequence]         AS AnswerSequence,
                                                     (
                                                         SELECT
                                                             CASE
                                                                 WHEN AnswerGuides.AnswerTypeId = 3
                                                                     THEN
                                                                     AnswerGuides.[Value]
                                                                 ELSE
                                                                     NULL
                                                             END
                                                     )                               AS AnswerGuidePattern
                                             FROM
                                                     [forms].QS
                                                 INNER JOIN
                                                     [forms].QSSectionMappings
                                                         ON QS.QSId = QSSectionMappings.QSId
                                                 INNER JOIN
                                                     [forms].Sections
                                                         ON Sections.SectionId = QSSectionMappings.SectionId
                                                 INNER JOIN
                                                     [forms].SectionFieldMappings
                                                         ON SectionFieldMappings.SectionId = QSSectionMappings.SectionId
                                                 INNER JOIN
                                                     [forms].Fields
                                                         ON SectionFieldMappings.FieldId = Fields.FieldId
                                                            AND SectionFieldMappings.FieldNo = Fields.FieldNo
                                                 LEFT JOIN
                                                     [forms].AnswerGuides
                                                         ON AnswerGuides.FieldId = Fields.FieldId
                                                            AND AnswerGuides.AnswerTypeId <> 4
                                             WHERE
                                                     QS.QSId = @QuestionSetId
                                             ORDER BY
                                                     QSId,
                                                     SectionId,
                                                     FieldId,
                                                     AnswerGuideId,
                                                     SectionSequence,
                                                     FieldSequence,
                                                     AnswerSequence
                                         END";
            questionSetByIdSql = questionSetByIdSql.Replace("'", "''");
            migrationBuilder.Sql($"EXEC ('{questionSetByIdSql}')");

            var getQuestionSetFieldConstraintById = @"CREATE PROCEDURE [dbo].[GetQuestionSetFieldConstraintById]
                                                    
                                                    @QuestionSetId UNIQUEIDENTIFIER
                                                    
                                                    AS
                                                    BEGIN
                                                                    
                                                    				SELECT DISTINCT FieldConstraintId	
                                                    				                ,TempFieldConstraint.FieldId	
                                                    				                ,TempFieldConstraint.SectionId	
                                                    				                ,DependantAnswerFieldId	
                                                    				                ,AnswerValue	
                                                    				 FROM ( SELECT FC.FieldConstraintId
                                                        						   ,FC.FieldId
                                                        						   --,FC.DependantAnswerGuideId
                                                        						   ,FC.SectionId
                                                        						   ,CONVERT(nvarchar(50), AG.FieldId) + '-' + convert(nvarchar(50), FieldNo) AS DependantAnswerFieldId
                                                    							   , AG.[Value] AS AnswerValue
                                                                        FROM   [Forms].FieldConstraints FC
                                                                        INNER JOIN [Forms].AnswerGuides AG
                                                                        ON FC.DependantAnswerGuideId = AG.AnswerGuideId
                                                                        INNER JOIN [Forms].Fields
                                                                        ON AG.FieldId = Fields.FieldId) as TempFieldConstraint
                                                                       INNER JOIN (SELECT QSId, 
                                                    									  SFM.Sectionid, 
                                                    									  SFM.FieldId 
                                                    					from [Forms].QSSectionMappings QSM						
                                                    					INNER JOIN [Forms].SectionFieldMappings SFM
                                                    							ON  QSM.SectionId =SFM.SectionId
                                                    					WHERE   QSId =@QuestionSetId) AS  TempQS
                                                    				ON TempFieldConstraint.SectionId = TempQS.SectionId 
                                                    					OR TempFieldConstraint.FieldId=TempQS.FieldId
                                                    
                                                    END
                                                    ";
            getQuestionSetFieldConstraintById = getQuestionSetFieldConstraintById.Replace("'", "''");
            migrationBuilder.Sql($"EXEC ('{getQuestionSetFieldConstraintById}')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var questionSetByIdSql = "DROP PROCEDURE [dbo].[GetQuestionSetById];";
            questionSetByIdSql = questionSetByIdSql.Replace("'", "''");
            migrationBuilder.Sql($"EXEC ('{questionSetByIdSql}')");

            var getQuestionSetFieldConstraintById = "DROP PROCEDURE [dbo].[GetQuestionSetFieldConstraintById];";
            getQuestionSetFieldConstraintById = getQuestionSetFieldConstraintById.Replace("'", "''");
            migrationBuilder.Sql($"EXEC ('{getQuestionSetFieldConstraintById}')");
        }
    }
}
