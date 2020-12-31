cd ../dbscript/workview
for /r %%G in (*.sql) do (
    sqlcmd -S "(localdb)\MSSQLLocalDB" -d zxsccore -i "%%G"
)
pause