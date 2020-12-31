IF EXISTS(SELECT * FROM sys.views WHERE [name]='v_fieldfilter')
DROP VIEW v_fieldfilter
GO

CREATE VIEW v_fieldfilter
AS
SELECT * FROM fieldfilter_fg