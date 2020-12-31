cd ../dbscript/backview
for /r %%G in (*.sql) do (
    sqlcmd -S "(localdb)\MSSQLLocalDB" -d zxscsyssetcore -i "%%G"
)
pause