﻿INSERT INTO dbo.DrawEntries (DrawId, EntrantId, RegisteredOn)
SELECT 12, e.Id, GETDATE()
FROM Entrants e
WHERE e.Id IN (1, 5, 9, 13, 17)