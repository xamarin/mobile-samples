Option Explicit On
''' <summary>
''' This interface is kinda redundant in the Xml example
''' but makes more sense in the SQLite.NET version of this
''' app, where in larger samples it allows us to write 
''' some Generic C.R.U.D. code where the ID is the Primary Key
''' </summary>
Public Interface IBusinessEntity
    Property ID() As Integer
End Interface
