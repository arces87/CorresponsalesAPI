
MERGE INTO [Seguridad].[RolMenu] AS Target
USING(VALUES
	(N'9d765f92-00b6-4f9f-313b-08d756658d8a', N'c3399529-96f3-4014-94ce-bf36ee695546', N'f0d637ba-06fe-4071-9790-6810f34ef228', 1),
	(N'666c76df-93c5-41f2-313c-08d756658d8a', N'c3399529-96f3-4014-94ce-bf36ee695546', N'6f992c56-e65c-4e6e-d161-08d754ceb622', 1),
	(N'b8f6950e-3a97-4745-313d-08d756658d8a', N'c3399529-96f3-4014-94ce-bf36ee695546', N'8a53faa7-3b44-4655-d162-08d754ceb622', 1),
	(N'41cf9f63-ee57-4fbb-313e-08d756658d8a', N'c3399529-96f3-4014-94ce-bf36ee695546', N'b125056c-1681-4b49-d163-08d754ceb622', 1),
	(N'543fe365-9bc0-42d9-a32b-08d76eae72f6', N'f0d637ba-06fe-4071-9790-6810f34ef227', N'f0d637ba-06fe-4071-9790-6810f34ef227', 1),
	(N'45f6cf53-1b17-486a-a32c-08d76eae72f6', N'f0d637ba-06fe-4071-9790-6810f34ef227', N'361c7810-659b-4750-d15e-08d754ceb622', 1),
	(N'afb0242e-1a6d-4926-a32d-08d76eae72f6', N'f0d637ba-06fe-4071-9790-6810f34ef227', N'f0d637ba-06fe-4071-9790-6810f34ef327', 1),
	(N'c4f72a38-8dd1-423e-a32e-08d76eae72f6', N'f0d637ba-06fe-4071-9790-6810f34ef227', N'f0d637ba-06fe-4071-9790-6810f34ef347', 1),
	(N'e189ff41-3ac8-43ae-a32f-08d76eae72f6', N'f0d637ba-06fe-4071-9790-6810f34ef227', N'f0d637ba-06fe-4071-9790-6810f34ef257', 1),
	(N'880a06da-0241-4975-a330-08d76eae72f6', N'f0d637ba-06fe-4071-9790-6810f34ef227', N'd8786dc8-8d89-408a-d15f-08d754ceb622', 1),
	(N'9e6663ad-c64c-4d33-a331-08d76eae72f6', N'f0d637ba-06fe-4071-9790-6810f34ef227', N'f0d637ba-06fe-4071-9790-6810f34ef228', 1),
	(N'c5fed07a-c967-48db-a332-08d76eae72f6', N'f0d637ba-06fe-4071-9790-6810f34ef227', N'e8432df4-0084-49f0-d160-08d754ceb622', 1),
	(N'c3e2de95-8df2-4811-a333-08d76eae72f6', N'f0d637ba-06fe-4071-9790-6810f34ef227', N'6f992c56-e65c-4e6e-d161-08d754ceb622', 1),
	(N'b86272ee-50ee-4bdc-a334-08d76eae72f6', N'f0d637ba-06fe-4071-9790-6810f34ef227', N'8a53faa7-3b44-4655-d162-08d754ceb622', 1),
	(N'd05507d8-fb84-4d42-a335-08d76eae72f6', N'f0d637ba-06fe-4071-9790-6810f34ef227', N'b125056c-1681-4b49-d163-08d754ceb622', 1),
	(N'0323a80e-b9e7-4e9c-a336-08d76eae72f6', N'f0d637ba-06fe-4071-9790-6810f34ef227', N'0a6d79dd-21a4-438e-cded-08d76eae6d8d', 1)
) 
AS Source([Id], [RolId], [MenuId], [EstaActivo])
ON Target.Id = Source.Id
    WHEN MATCHED
    THEN UPDATE SET 
                    [RolId] = Source.[RolId], 
                    [MenuId] = Source.[MenuId],
                    [EstaActivo] = Source.[EstaActivo]
    WHEN NOT MATCHED BY TARGET
    THEN
      INSERT ([Id], [RolId], [MenuId], [EstaActivo])
      VALUES ([Id], [RolId], [MenuId], [EstaActivo])
    WHEN NOT MATCHED BY SOURCE
    THEN DELETE;


