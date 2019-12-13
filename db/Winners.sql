SELECT 
d.Name AS NombreSorteo,
p.Id AS IdPremio,
p.Name AS NombrePremio,
e.Code AS CedulaGanador,
e.Name AS NombreGanador,
(SELECT COUNT(*) FROM DrawEntries WHERE EntrantId = e.Id AND DrawId = d.Id) AS NumeroParticipaciones,
e.Office AS OficinaGanador,
e.Unit AS UnidadGanador,
e.SubDepartment AS SubDepartamentoGanador,
e.City AS CiudadGanador
FROM PrizeSelectionSteps pss
	INNER JOIN Entrants e ON e.Id = pss.EntrantId
	INNER JOIN Prizes p ON p.Id = pss.PrizeId
	INNER JOIN Draws d ON d.Id = p.DrawId
	INNER JOIN DrawEntries de ON de.Id = pss.DrawEntryId
	