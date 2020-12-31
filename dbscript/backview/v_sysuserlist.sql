IF EXISTS(SELECT * FROM sys.views WHERE [name]='v_sysuserlist')
DROP VIEW v_sysuserlist
GO

CREATE VIEW v_sysuserlist
AS
    SELECT *
    FROM sysuserlist_ul