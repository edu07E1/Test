TripsCore
---------

Solution description
This is a VS2019 .NET Core and Angular project solution, comprising the following projects:
  DALib: Data access library, has an abtract class DAClass which standarices data interface.
  It implements DA_Net class which manages data processing using .NET Core lists.
  Further data processing options could be implemented thru derivations of DAClass, ie. SQL Server connectivity in order to rely all data processing to the SQL.

  TripsCore: is the main application project, it has the HomeController with the logic for file upload management.
  DataFile class implements the file basic parsing and security validations.

  ClientApp: Angular client application, implementing the home.component as the user interface.
  (file of ClientApp/node_modules where uploaded in one zip file to GitHub)

ADDITIONAL BUSINESS RULES: these rules where included in the logic to cover situations not specified in the original requirement (in a real project any unspecified rule would be consulted with the appropriate stakeholders during design and the project specifications updated with the new information.)
a. Accepted file extensions: .dat .txt
b. Trips from unregistered drivers will be discarded.
c. Text lines with unrecognized commands will be discarded.
d. Text lines with incorrect number of fields and/or invalid data will be discarded.
e. Text lines longer than 80 characters causes the whole process to abort.

	
