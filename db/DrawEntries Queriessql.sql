INSERT INTO dbo.DrawEntries (DrawId, EntrantId, RegisteredOn)
SELECT 6, e.Id, GETDATE()
FROM Entrants e
--WHERE e.Id IN (5, 8, 10, 12, 15)