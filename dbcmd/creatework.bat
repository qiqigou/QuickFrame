cd ../QuickFrame.Web
dotnet ef migrations add CreateWorkDatabase --context workdbcontext --project ../QuickFrame.Models --output-dir Migrations/WorkDb
dotnet ef database update --context workdbcontext --project ../QuickFrame.Models
pause