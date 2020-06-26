namespace TQ.Core.Constants
{
    /// <summary>
    /// Invalid Inputs for code sanitization
    /// </summary>
    public static class InvalidInputConstants
    {
        /// <summary>
        /// Specify list of strings for input injection
        /// </summary>
        public static readonly string[] InvalidData = new string[]
        {
            "drop table",
            "drop database",
            "select * from",
            "select top 1",
            "insert into",
            "union select",
            "select if",
            "union all select",
            "select sleep(",
            "select sleep (",
            "substring",
            "row_count()",
            "/*",
            "select concat(",
            "select concat (",
            "truncate table",
            "delete * from ",
            "delete from",
            "if exists",
            "alter table",
            "1=1",
            "console.log(",
            "console.log (",
            "alert (",
            "alert(",
            "javascript",
            "document.cookie",
            "background-image",
            "void(",
            "void (",
            "onclick=",
            "document.getElementById",
            "alter database",
            "alter table",
            "waitfor delay",
            "create index",
            "drop index",
            "create view",
            "select distinct",
            "backup database",
            "sql",
            "create table"
        };

        /// <summary>
        /// Regular expression to find the xml and html injection
        /// </summary>
        public static readonly string InvalidDataRegex = @"<(.|\n)*?>";
    }
}