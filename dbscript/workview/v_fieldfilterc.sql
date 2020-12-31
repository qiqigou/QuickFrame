IF EXISTS(SELECT * FROM sys.views WHERE [name]='v_fieldfilterc')
DROP VIEW v_fieldfilterc
GO

CREATE VIEW v_fieldfilterc
AS
SELECT * FROM fieldfilterc_fgc