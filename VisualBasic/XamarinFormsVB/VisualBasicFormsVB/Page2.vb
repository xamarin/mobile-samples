Imports Xamarin.Forms

Public Class Page2
    Inherits ContentPage

    Public Sub New()
        Dim label = New Label With {.HorizontalTextAlignment = TextAlignment.Center,
                                    .FontSize = Device.GetNamedSize(NamedSize.Medium, GetType(Label)),
                                    .Text = "Visual Basic ContentPage"}

        Dim stack = New StackLayout With {
            .VerticalOptions = LayoutOptions.Center
        }
        stack.Children.Add(label)

        Content = stack
    End Sub
End Class