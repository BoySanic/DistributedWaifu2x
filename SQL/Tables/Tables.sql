create table dbo.Donor(
DonorID int identity(1,1),
DonorName varchar(30),
DonorTeamID int
)
create table dbo.DonorTeam(
DonorTeamID int identity(1,1),
DonorTeamName varchar(max),
DonorTeamURL varchar(max)
)
create table dbo.Project(
ProjectID int identity(1,1),
ProjectServerPath varchar(max),
ProjectInFolder varchar(max),
ProjectOutFolder varchar(max),
ProjectFrameAmount int,
OriginalHeight int,
OriginalWidth int,
ScalingRatio float
)
create table dbo.Package(
PackageID int identity(1,1),
ProjectID int,
PackageWorth float,
CompletionDonorID int,
CompletionNodeID int,
SentTime datetime,
NodeStartTime datetime,
NodeEndtime datetime,
ReceivedTime datetime,
ExpiryTime datetime
)
create table dbo.Frame(
FrameID int identity(1,1),
PackageID int,
FrameFileName varchar(max)
)
create table dbo.PackageHistory(
PackageHistID int identity(1,1),
PackageID int,
OldNodeID int,
NewNodeID int,
Reason varchar(max),
ChangeOccured datetime
)
create table dbo.Node(
NodeID int identity(1,1),
HostName varchar(max),
DevName varchar(max),
MeanTimeBetweenCompletions float
)