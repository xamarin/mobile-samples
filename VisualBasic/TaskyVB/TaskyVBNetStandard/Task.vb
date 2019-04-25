Option Explicit On
''' <summary>
''' This is our business object for the to-do list
''' </summary>
Public Class Task
    Implements IBusinessEntity

    Property ID() As Integer Implements IBusinessEntity.ID

    Property Name() As String

    Property Notes() As String

    Property Done() As Boolean

End Class
