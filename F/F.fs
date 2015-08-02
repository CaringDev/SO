module Test
 open Microsoft.FSharp.Data.TypeProviders
 type internal DB = SqlDataConnection<"Data Source=.;Initial Catalog=OBSCURED;Integrated Security=SSPI;">
 let private entities = DB.GetDataContext().OBSCURED

 type dbml = DbmlFile<"MyDatabase.dbml", ContextTypeName = "MyDataContext">