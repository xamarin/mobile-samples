Option Explicit On
''' <summary>
''' Abstract manager for Task C.R.U.D.
''' </summary>
Public Class TaskManager

    Private _repository As TaskRepository

    Public Sub New(filename As String, storage As IXmlStorage)
        _repository = New TaskRepository(filename, storage)
    End Sub

    Public Function GetTask(id As Integer) As Task
        Return _repository.GetTask(id)
    End Function

    Public Function GetTasks() As IList(Of Task)
        Return New List(Of Task)(_repository.GetTasks())
    End Function

    Public Function SaveTask(item As Task) As Integer
        Return _repository.SaveTask(item)
    End Function

    Public Function DeleteTask(id As Integer) As Integer
        Return _repository.DeleteTask(id)
    End Function
End Class
