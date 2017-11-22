Option Explicit On
''' <summary>
''' Implementation of Task (to-do item) storage
''' </summary>
''' <remarks>
''' Loads and saves the Task objects as an XML file.
''' Because Portable Class Libraries cannot reference System.IO we have to 
''' pass in a class (via Interface) that loads and saves the xml file for us
''' </remarks>
Public Class TaskRepository

    Private _filename As String
    Private _storage As IXmlStorage
    Private _tasks As List(Of Task)

    ''' <summary>
    ''' Constructor (for those who know C# better than VB)
    ''' </summary>
    Public Sub New(filename As String, storage As IXmlStorage)
        _filename = filename
        _storage = storage
        _tasks = _storage.ReadXml(filename)
    End Sub
    ''' <summary>
    ''' Inefficient search for a Task by ID
    ''' </summary>
    Public Function GetTask(id As Integer) As Task
        For t As Integer = 0 To _tasks.Count - 1
            If _tasks(t).ID = id Then
                Return _tasks(t)
            End If

        Next
        Return New Task() With {.ID = id}
    End Function
    ''' <summary>
    ''' List all the Tasks 
    ''' </summary>
    Public Function GetTasks() As IEnumerable(Of Task)
        Return _tasks
    End Function
    ''' <summary>
    ''' Save a Task to the Xml file
    ''' Calculates the ID as the max of existing IDs
    ''' </summary>
    Public Function SaveTask(item As Task) As Integer
        Dim max As Integer = 0

        If _tasks.Count > 0 Then
            max = _tasks.Max(Function(t As Task) t.ID)
        End If
        If item.ID = 0 Then
            item.ID = ++max
            _tasks.Add(item)
        Else
            ''HACK: why isn't Find available in PCL?
            Dim j = _tasks.Where(Function(t) t.ID = item.ID).First()
            j = item
        End If
        _storage.WriteXml(_tasks, _filename)
        Return max
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    Public Function DeleteTask(id As Integer) As Integer
        For t As Integer = 0 To _tasks.Count - 1
            If _tasks(t).ID = id Then
                _tasks.RemoveAt(t)
                _storage.WriteXml(_tasks, _filename)
                Return 1
            End If
        Next
        Return -1
    End Function

End Class
