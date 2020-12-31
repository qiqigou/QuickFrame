IF EXISTS(SELECT * FROM sys.views WHERE [name]='v_syscompany')
DROP VIEW v_syscompany
GO

CREATE VIEW v_syscompany
AS
    SELECT *
    FROM syscompany_scy