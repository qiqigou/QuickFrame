cd ../QuickFrame.Web
dotnet ef migrations add CreateBackDatabase --context backdbcontext --project ../QuickFrame.Model --output-dir Migrations/BackDb
dotnet ef database update --context backdbcontext --project ../QuickFrame.Model
pause