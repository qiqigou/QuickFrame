IF EXISTS(SELECT * FROM sys.views WHERE [name]='v_syscompanyshop')
DROP VIEW v_syscompanyshop
GO

CREATE VIEW v_syscompanyshop
AS
SELECT * FROM syscompanyshop_scs