SELECT COUNT(*) FROM Communities;

DELETE FROM Communities
FROM
	Communities c
LEFT OUTER JOIN
	IncidentTasks it
	ON c.Id = it.CommunityId
LEFT OUTER JOIN
	Addresses a
	ON c.Id = a.CommunityId
LEFT OUTER JOIN
	Organizations o
	ON c.Id = o.CommunityId
LEFT OUTER JOIN
	Registrations r
	ON c.Id = r.HostCommunityId
WHERE 
	it.Id IS NULL AND
	a.Id IS NULL AND
	o.Id IS NULL AND
	r.Id IS NULL

SELECT COUNT(*) FROM Communities;
