CREATE TABLE [dbo].[Flights] (
    [FlightId] INT           IDENTITY (1, 1) NOT NULL,
    [Active]   BIT           NULL,
    [Name]     NVARCHAR (50) NULL,
    CONSTRAINT [PK_Flights] PRIMARY KEY CLUSTERED ([FlightId] ASC)
);

