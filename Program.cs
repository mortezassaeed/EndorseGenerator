// See https://aka.ms/new-console-template for more information
Console.WriteLine("TPT(0) or TPH(1)");
var tableType = Console.ReadLine();


Console.WriteLine("Enter schema and table name:");
string tableName = Console.ReadLine();

var tableNameDetail = tableName.Split('.');
if (tableType.ToString() == "1")
{
    Console.WriteLine("--current :");
    string currentScript = @$"create view [dbo].[vw_{tableNameDetail[0]}_{tableNameDetail[1]}_current]
                            as
                            select * from {tableName} where IsCurrent=1";

    Console.WriteLine(currentScript);
    Console.WriteLine("--first :");
    string firstScript = @$"create view [dbo].[vw_{tableNameDetail[0]}_{tableNameDetail[1]}_first]
                            as
                            select * from {tableName} where FromEndorseNo=0";

    Console.WriteLine(firstScript);

    Console.WriteLine("--latest :");
    string latestScript = @$"create view [dbo].[vw_{tableNameDetail[0]}_{tableNameDetail[1]}_latest]
                            as
                            select * from {tableName} where ToEndorseNo is null";

    Console.WriteLine(latestScript);
    
    Console.WriteLine("--fn version :");
    string versionScript = @$"create view [dbo].[fn_{tableNameDetail[0]}_{tableNameDetail[1]}_ver_eq]
                            returns table
                            as return
                            select * from {tableName} where FromEndorseNo = @ver";
    Console.WriteLine(versionScript);

    Console.WriteLine("--fn match date :");
    string matchDateScript = @$"CREATE function [dbo].[fn_{tableNameDetail[0]}_{tableNameDetail[1]}_ver_match_date](@asOfDate dtDate)
                                returns table
                                as return
                                with Endorse as (
                                select PolicyID, max(EndorseNo) EndorseNo 
                                from cmn.Endorse
                                where endorse.EffectDate <= @asOfDate
                                group by PolicyID)
                                select policy.* from {tableName} policy 
                                left join Endorse endorse on endorse.PolicyID=policy.ID and endorse.EndorseNo=policy.FromEndorseNo
                                where (policy.FromEndorseNo = 0 and policy.ToEndorseNo is null and policy.RegisterDate <= @asOfDate or endorse.EndorseNo is not null)";

}
else if(tableType.ToString() == "1")
{
    Console.WriteLine("Enter parent table name:");
    string tableParentName = Console.ReadLine();


}


