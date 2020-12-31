IF EXISTS(SELECT * FROM sys.views WHERE [name]='v_userinfo')
DROP VIEW v_userinfo
GO

--用户信息视图
CREATE VIEW v_userinfo
AS
    SELECT *
    FROM userinfo_us
