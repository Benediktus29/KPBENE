Public Class Form2
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form3.Show()
        Me.Hide()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) 
        Dim intResponse As Integer
        intResponse = MsgBox("Anda Yakin Ingin Keluar ?",
        vbYesNo + vbQuestion, "Peringatan")
        If intResponse = vbYes Then
            Me.Close()
            Form1.Show()
            MsgBox("Anda Berhasil Keluar", MsgBoxStyle.MsgBoxRight, "Perhatian")
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Form5.Show()
        Me.Hide()
    End Sub
End Class