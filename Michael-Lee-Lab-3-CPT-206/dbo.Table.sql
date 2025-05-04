CREATE TABLE [dbo].[States]
(
    [StateID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [StateName] NVARCHAR(100) NOT NULL UNIQUE,
    [Population] INT NOT NULL,
    [FlagDescription] NVARCHAR(MAX) NULL,
    [StateFlower] NVARCHAR(100) NULL,
    [StateBird] NVARCHAR(100) NULL,
    [Colors] NVARCHAR(100) NULL,
    [LargestCities] NVARCHAR(MAX) NULL,
    [StateCapital] NVARCHAR(100) NOT NULL,
    [MedianIncome] DECIMAL(10,2) NOT NULL,
    [ComputerJobsPercent] DECIMAL(5,2) NOT NULL
);
