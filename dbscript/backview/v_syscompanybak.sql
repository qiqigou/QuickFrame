IF EXISTS(SELECT * FROM sys.views WHERE [name]='v_syscompanybak')
DROP VIEW v_syscompanybak
GO

CREATE VIEW v_syscompanybak
AS
SELECT * FROM syscompanybak_scb