CREATE NONCLUSTERED INDEX [IX_Outlet_OutletTypeID] ON [dbo].[Outlet]
(
	[OutletTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
