cd ../QuickFrame.Web
dotnet ef migrations add CreateBackDatabase --context backdbcontext --project ../QuickFrame.Models --output-dir Migrations/BackDb
dotnet ef database update --context backdbcontext --project ../QuickFrame.Models
pause