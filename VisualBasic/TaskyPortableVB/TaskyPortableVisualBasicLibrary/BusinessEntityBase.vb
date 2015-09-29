Option Explicit On
''' <summary>
''' This base class is kinda redundant in the Xml example
''' but makes more sense in the SQLite.NET version of this
''' app, where in larger samples it allows us to write 
''' some Generic C.R.U.D. code where the ID is the Primary Key
''' </summary>
Public Class BusinessEntityBase
    Implements IBusinessEntity

    Property ID() As Integer Implements IBusinessEntity.ID

End Class
