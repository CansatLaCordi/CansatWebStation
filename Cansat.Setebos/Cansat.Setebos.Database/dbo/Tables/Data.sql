CREATE TABLE [dbo].[Data] (
    [DataId]              INT              IDENTITY (1, 1) NOT NULL,
    [FlightId]            INT              NOT NULL,
    [Datetime]            DATETIME         NULL,
    [Temperature]         DECIMAL (18, 8)  NULL,
    [Latitude]            DECIMAL (18, 10) NULL,
    [Longitude]           DECIMAL (18, 10) NULL,
    [Altitude]            DECIMAL (18, 8)  NULL,
    [Humidity]            DECIMAL (18, 8)  NULL,
    [Presure]             DECIMAL (18, 8)  NULL,
    [Voltage]             DECIMAL (18, 8)  NULL,
    [CO]                  DECIMAL (18, 10) NULL,
    [InternalTemperature] DECIMAL (18, 8)  NULL,
    CONSTRAINT [PK_Data] PRIMARY KEY CLUSTERED ([DataId] ASC),
    CONSTRAINT [FK_Data_Flights] FOREIGN KEY ([FlightId]) REFERENCES [dbo].[Flights] ([FlightId])
);

